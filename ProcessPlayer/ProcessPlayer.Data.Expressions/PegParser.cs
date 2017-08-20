using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace ProcessPlayer.Data.Expressions
{
    public abstract class PegParser
    {
        #region private variables

        private PegTree _tree;
        private readonly List<int> _lineStarts = new List<int> { 0 };

        #endregion

        #region protected variables

        protected bool _mute;
        protected delCreator _nodeCreator;
        protected int _pos;
        protected int _srcLen;
        protected TextWriter _errOut;

        #endregion

        #region private methods

        private void AddTreeNode(int id, AddPolicy newAddPolicy, delCreator createNode, ECreatorPhase ePhase)
        {
            if (_mute)
                return;

            if (_tree.root == null)
                _tree.root = _tree.cur = createNode(ePhase, _tree.cur, id);
            else if (_tree.addPolicy == AddPolicy.eAddAsChild)
                _tree.cur = _tree.cur.child = createNode(ePhase, _tree.cur, id);
            else
                _tree.cur = _tree.cur.next = createNode(ePhase, _tree.cur.parent, id);

            _tree.addPolicy = newAddPolicy;
        }

        private void RestoreTree(PegNode prevCur, AddPolicy prevPolicy)
        {
            if (_mute)
                return;

            if (prevCur == null)
                _tree.root = null;
            else if (prevPolicy == AddPolicy.eAddAsChild)
                prevCur.child = null;
            else
                prevCur.next = null;

            _tree.cur = prevCur;
            _tree.addPolicy = prevPolicy;
        }

        #endregion

        #region protected methods

        protected PegNode DefaultNodeCreator(ECreatorPhase phase, PegNode parentOrCreated, int id)
        {
            if (phase == ECreatorPhase.eCreate || phase == ECreatorPhase.eCreateAndComplete)
                return new PegNode(parentOrCreated, id);
            
            return null;
        }

        protected void ResetLineStarts()
        {
            _lineStarts.Clear();
            _lineStarts.Add(0);
        }

        #endregion

        #region public methods

        public bool And(delMatcher pegSequence)
        {
            AddPolicy prevPolicy = _tree.addPolicy;
            PegNode prevCur = _tree.cur;
            int pos0 = _pos;

            bool matches = pegSequence();

            if (!matches)
            {
                _pos = pos0;

                RestoreTree(prevCur, prevPolicy);
            }

            return matches;
        }

        public bool Any()
        {
            if (_pos < _srcLen)
            {
                _pos++;

                return true;
            }

            return false;
        }

        public void Construct(TextWriter errOut)
        {
            _mute = false;
            _srcLen = _pos = 0;

            SetErrorDestination(errOut);

            ResetTree();
        }

        public bool ForRepeat(int count, delMatcher toRepeat)
        {
            AddPolicy prevPolicy = _tree.addPolicy;
            PegNode prevCur = _tree.cur;
            int i, pos0 = _pos;

            for (i = 0; i < count; ++i)
            {
                if (!toRepeat())
                {
                    _pos = pos0;

                    RestoreTree(prevCur, prevPolicy);

                    return false;
                }
            }

            return true;
        }

        public bool ForRepeat(int lower, int upper, delMatcher toRepeat)
        {
            AddPolicy prevPolicy = _tree.addPolicy;
            PegNode prevCur = _tree.cur;
            int i, pos0 = _pos;

            for (i = 0; i < upper; ++i)
                if (!toRepeat())
                    break;

            if (i < lower)
            {
                _pos = pos0;

                RestoreTree(prevCur, prevPolicy);

                return false;
            }

            return true;
        }

        public void GetLineAndCol(string s, int pos, out int lineNo, out int colNo)
        {
            var curPos = 0;
            if (_lineStarts.Count > 0)
            {
                lineNo = _lineStarts.Count;
                curPos = _lineStarts[_lineStarts.Count - 1];
                if (curPos == pos)
                {
                    colNo = 1;
                    return;
                }
            }

            colNo = 2;

            for (int i = curPos + 1; i <= pos; ++i, ++colNo)
            {
                if (s[i - 1] == '\n')
                {
                    _lineStarts.Add(i);
                    colNo = 1;
                }
            }

            colNo--;

            lineNo = _lineStarts.Count;
        }

        public IEnumerable<PegNode> GetNodeChildren(PegNode node)
        {
            if ((node = node.child) != null)
                do
                {
                    yield return node;

                    node = node.next;
                }
                while (node != null);
        }

        public PegNode GetRoot()
        {
            return _tree.root;
        }

        public bool Not(delMatcher toMatch)
        {
            bool prevMute = _mute;
            int pos0 = _pos;

            _mute = true;

            bool matches = toMatch();

            _mute = prevMute;
            _pos = pos0;

            return !matches;
        }

        public bool Option(delMatcher toMatch)
        {
            int pos0 = _pos;

            if (!toMatch())
                _pos = pos0;

            return true;
        }

        public bool Option(delSpaceMatcher toMatch, ref int spaceCount, int minSpaceCount)
        {
            int pos0 = _pos;

            if (!toMatch(ref spaceCount, minSpaceCount))
                _pos = pos0;

            return true;
        }

        public bool OptRepeat(delMatcher toRepeat)
        {
            for (; ; )
            {
                int pos0 = _pos;

                if (!toRepeat())
                {
                    _pos = pos0;

                    return true;
                }
            }
        }

        public bool OptRepeat(delSpaceMatcher toRepeat, ref int spaceCount, int minSpaceCount)
        {
            for (; ; )
            {
                int pos0 = _pos;

                if (!toRepeat(ref spaceCount, minSpaceCount))
                {
                    _pos = pos0;

                    return true;
                }
            }
        }

        public bool Peek(delMatcher toMatch)
        {
            bool prevMute = _mute;
            int pos0 = _pos;

            _mute = true;

            bool matches = toMatch();

            _mute = prevMute;
            _pos = pos0;

            return matches;
        }

        public bool PlusRepeat(delMatcher toRepeat)
        {
            int i;

            for (i = 0; ; i++)
            {
                int pos0 = _pos;

                if (!toRepeat())
                {
                    _pos = pos0;

                    break;
                }
            }

            return i > 0;
        }

        public void ResetTree()
        {
            _tree.addPolicy = AddPolicy.eAddAsChild;
            _tree.cur = null;
            _tree.root = null;
        }

        public void Rewind()
        {
            _pos = 0;
        }

        public void SetErrorDestination(TextWriter errOut)
        {
            _errOut = errOut ?? Console.Error;
        }

        public void SetNodeCreator(delCreator nodeCreator)
        {
            if (nodeCreator == null)
                throw new NullReferenceException("nodeCreator can't be null.");

            _nodeCreator = nodeCreator;
        }

        public bool TreeAST(delCreator nodeCreator, delMatcher toMatch)
        {
            return TreeAST(nodeCreator, (int)ESpecialNodes.eAnonymASTNode, toMatch);
        }

        public bool TreeAST(delCreator nodeCreator, int ruleId, delMatcher toMatch)
        {
            if (_mute)
                return toMatch();

            var matches = TreeNT(nodeCreator, ruleId, toMatch);

            if (matches)
            {
                if (_tree.cur.child != null && _tree.cur.child.next == null && _tree.cur.parent != null)
                {
                    if (_tree.cur.parent.child == _tree.cur)
                    {
                        _tree.cur.child.parent = _tree.cur.parent;
                        _tree.cur.parent.child = _tree.cur.child;
                        _tree.cur = _tree.cur.child;
                    }
                    else
                    {
                        PegNode prev;

                        for (prev = _tree.cur.parent.child; prev != null && prev.next != _tree.cur; prev = prev.next)
                        {
                        }

                        if (prev != null)
                        {
                            prev.next = _tree.cur.child;

                            _tree.cur.child.parent = _tree.cur.parent;
                            _tree.cur = _tree.cur.child;
                        }
                    }
                }
            }

            return matches;
        }

        public bool TreeAST(delMatcher toMatch)
        {
            return TreeAST((int)ESpecialNodes.eAnonymASTNode, toMatch);
        }

        public bool TreeAST(int ruleId, delMatcher toMatch)
        {
            return TreeAST(_nodeCreator, ruleId, toMatch);
        }

        public bool TreeChars(delCreator nodeCreator, delMatcher toMatch)
        {
            return TreeCharsWithId(nodeCreator, (int)ESpecialNodes.eAnonymousNode, toMatch);
        }

        public bool TreeChars(delMatcher toMatch)
        {
            return TreeCharsWithId((int)ESpecialNodes.eAnonymousNode, toMatch);
        }

        public bool TreeCharsWithId(int id, delMatcher toMatch)
        {
            return TreeCharsWithId(_nodeCreator, id, toMatch);
        }

        public bool TreeCharsWithId(delCreator nodeCreator, int id, delMatcher toMatch)
        {
            var pos = _pos;

            if (toMatch())
            {
                if (!_mute)
                {
                    AddTreeNode(id, AddPolicy.eAddAsSibling, nodeCreator, ECreatorPhase.eCreateAndComplete);

                    _tree.cur.match.posBeg = pos;
                    _tree.cur.match.posEnd = _pos;
                }

                return true;
            }

            return false;
        }

        public bool TreeNT(delCreator nodeCreator, delMatcher toMatch)
        {
            return TreeNT(nodeCreator, (int)ESpecialNodes.eAnonymNTNode, toMatch);
        }

        public bool TreeNT(delCreator nodeCreator, int ruleId, delMatcher toMatch)
        {
            if (_mute)
                return toMatch();

            AddPolicy prevPolicy = _tree.addPolicy;
            int posBeg = _pos;
            var prevCur = _tree.cur;

            AddTreeNode(ruleId, AddPolicy.eAddAsChild, nodeCreator, ECreatorPhase.eCreate);

            var ruleNode = _tree.cur;

            var matches = toMatch();

            if (!matches)
                RestoreTree(prevCur, prevPolicy);
            else
            {
                ruleNode.match.posBeg = posBeg;
                ruleNode.match.posEnd = _pos;

                _tree.addPolicy = AddPolicy.eAddAsSibling;
                _tree.cur = ruleNode;

                nodeCreator(ECreatorPhase.eCreationComplete, ruleNode, ruleId);
            }

            return matches;
        }

        public bool TreeNT(delMatcher toMatch)
        {
            return TreeNT((int)ESpecialNodes.eAnonymNTNode, toMatch);
        }

        public bool TreeNT(int ruleId, delMatcher toMatch)
        {
            return TreeNT(_nodeCreator, ruleId, toMatch);
        }

        #endregion

        #region virtual methods

        public virtual void GetProperties(out EncodingClass encoding, out UnicodeDetection detection)
        {
            encoding = EncodingClass.ascii;
            detection = UnicodeDetection.notApplicable;
        }

        public virtual string GetRuleNameFromId(int id)
        {
            switch (id)
            {
                case (int)ESpecialNodes.eAnonymASTNode:
                    return "ASTNode";
                case (int)ESpecialNodes.eAnonymNTNode:
                    return "Nonterminal";
                case (int)ESpecialNodes.eAnonymousNode:
                    return "Node";
                case (int)ESpecialNodes.eFatal:
                    return "FATAL";
                default:
                    return id.ToString(CultureInfo.InvariantCulture);
            }
        }

        public virtual string TreeNodeToString(PegNode node)
        {
            return GetRuleNameFromId(node.id);
        }

        #endregion

        #region constructors

        protected PegParser(TextWriter errOut)
        {
            _errOut = errOut;
            _nodeCreator = DefaultNodeCreator;
            _srcLen = _pos = 0;
        }

        #endregion
    }
}
