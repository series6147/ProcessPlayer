using System.Collections.Generic;
using System.IO;

namespace ProcessPlayer.Data.Expressions
{
    public class PegCharParser : PegBaseParser
    {
        #region protected variables

        protected string _src;

        #endregion

        #region overriden methods

        public override string TreeNodeToString(PegNode node)
        {
            string label = base.TreeNodeToString(node);

            if (node.id == (int)ESpecialNodes.eAnonymousNode)
            {
                string value = node.GetAsString(_src);

                if (value.Length < 32)
                    label += " <" + value + ">";
                else
                    label += " <" + value.Substring(0, 29) + "...>";
            }

            return label;
        }

        #endregion

        #region private methods

        private void LogOutMsg(string sErrKind, string sMsg)
        {
            int colNo, lineNo;

            GetLineAndCol(_src, _pos, out lineNo, out colNo);

            _errOut.WriteLine("<{0},{1}>{2}:{3}", lineNo, colNo, sErrKind, sMsg);
            _errOut.Flush();
        }

        #endregion

        #region public methods

        public bool Char(char c1)
        {
            if (_pos < _srcLen && _src[_pos] == c1)
            {
                _pos++;

                return true;
            }

            return false;
        }

        public bool Char(char c1, char c2)
        {
            if (_pos + 1 < _srcLen && _src[_pos] == c1 && _src[_pos + 1] == c2)
            {
                _pos += 2;

                return true;
            }

            return false;
        }

        public bool Char(char c1, char c2, char c3)
        {
            if (_pos + 2 < _srcLen && _src[_pos] == c1 && _src[_pos + 1] == c2 && _src[_pos + 2] == c3)
            {
                _pos += 3;

                return true;
            }

            return false;
        }

        public bool Char(char c1, char c2, char c3, char c4)
        {
            if (_pos + 3 < _srcLen && _src[_pos] == c1 && _src[_pos + 1] == c2 && _src[_pos + 2] == c3 && _src[_pos + 3] == c4)
            {
                _pos += 4;

                return true;
            }

            return false;
        }

        public bool Char(char c1, char c2, char c3, char c4, char c5)
        {
            if (_pos + 4 < _srcLen && _src[_pos] == c1 && _src[_pos + 1] == c2 && _src[_pos + 2] == c3 && _src[_pos + 3] == c4 && _src[_pos + 4] == c5)
            {
                _pos += 5;

                return true;
            }

            return false;
        }

        public bool Char(char c1, char c2, char c3, char c4, char c5, char c6)
        {
            if (_pos + 5 < _srcLen && _src[_pos] == c1 && _src[_pos + 1] == c2 && _src[_pos + 2] == c3 && _src[_pos + 3] == c4 && _src[_pos + 4] == c5 && _src[_pos + 5] == c6)
            {
                _pos += 6;

                return true;
            }

            return false;
        }

        public bool Char(char c1, char c2, char c3, char c4, char c5, char c6, char c7)
        {
            if (_pos + 6 < _srcLen && _src[_pos] == c1 && _src[_pos + 1] == c2 && _src[_pos + 2] == c3 && _src[_pos + 3] == c4 && _src[_pos + 4] == c5 && _src[_pos + 5] == c6 && _src[_pos + 6] == c7)
            {
                _pos += 7;

                return true;
            }

            return false;
        }

        public bool Char(char c1, char c2, char c3, char c4, char c5, char c6, char c7, char c8)
        {
            if (_pos + 7 < _srcLen && _src[_pos] == c1 && _src[_pos + 1] == c2 && _src[_pos + 2] == c3 && _src[_pos + 3] == c4 && _src[_pos + 4] == c5 && _src[_pos + 5] == c6 && _src[_pos + 6] == c7 && _src[_pos + 7] == c8)
            {
                _pos += 8;

                return true;
            }

            return false;
        }

        public bool Char(string s)
        {
            int length = s.Length;

            if (_pos + length > _srcLen)
                return false;

            for (int i = 0; i < length; ++i)
                if (s[i] != _src[_pos + i])
                    return false;

            _pos += length;

            return true;
        }

        public void Construct(string src, TextWriter errOut)
        {
            Construct(errOut);

            SetSource(src);
        }

        public virtual bool SyntaxError(string sMsg)
        {
            LogOutMsg("Syntax error", sMsg);

            int colNo, lineNo;
            GetLineAndCol(_src, _pos, out lineNo, out colNo);

            throw new PegException(FileName, "Syntax Error", sMsg, lineNo, colNo);
        }

        public string GetSource()
        {
            return _src;
        }

        public bool IChar(char c1)
        {
            if (_pos < _srcLen && System.Char.ToUpper(_src[_pos]) == c1)
            {
                _pos++;

                return true;
            }

            return false;
        }

        public bool IChar(char c1, char c2)
        {
            if (_pos + 1 < _srcLen && System.Char.ToUpper(_src[_pos]) == System.Char.ToUpper(c1) && System.Char.ToUpper(_src[_pos + 1]) == System.Char.ToUpper(c2))
            {
                _pos += 2;

                return true;
            }

            return false;
        }

        public bool IChar(char c1, char c2, char c3)
        {
            if (_pos + 2 < _srcLen && System.Char.ToUpper(_src[_pos]) == System.Char.ToUpper(c1) && System.Char.ToUpper(_src[_pos + 1]) == System.Char.ToUpper(c2) && System.Char.ToUpper(_src[_pos + 2]) == System.Char.ToUpper(c3))
            {
                _pos += 3;

                return true;
            }

            return false;
        }

        public bool IChar(char c1, char c2, char c3, char c4)
        {
            if (_pos + 3 < _srcLen && System.Char.ToUpper(_src[_pos]) == System.Char.ToUpper(c1) && System.Char.ToUpper(_src[_pos + 1]) == System.Char.ToUpper(c2) && System.Char.ToUpper(_src[_pos + 2]) == System.Char.ToUpper(c3) && System.Char.ToUpper(_src[_pos + 3]) == System.Char.ToUpper(c4))
            {
                _pos += 4;

                return true;
            }

            return false;
        }

        public bool IChar(char c1, char c2, char c3, char c4, char c5)
        {
            if (_pos + 4 < _srcLen && System.Char.ToUpper(_src[_pos]) == System.Char.ToUpper(c1) && System.Char.ToUpper(_src[_pos + 1]) == System.Char.ToUpper(c2) && System.Char.ToUpper(_src[_pos + 2]) == System.Char.ToUpper(c3) && System.Char.ToUpper(_src[_pos + 3]) == System.Char.ToUpper(c4) && System.Char.ToUpper(_src[_pos + 4]) == System.Char.ToUpper(c5))
            {
                _pos += 5;

                return true;
            }

            return false;
        }

        public bool IChar(char c1, char c2, char c3, char c4, char c5, char c6)
        {
            if (_pos + 5 < _srcLen && System.Char.ToUpper(_src[_pos]) == System.Char.ToUpper(c1) && System.Char.ToUpper(_src[_pos + 1]) == System.Char.ToUpper(c2) && System.Char.ToUpper(_src[_pos + 2]) == System.Char.ToUpper(c3) && System.Char.ToUpper(_src[_pos + 3]) == System.Char.ToUpper(c4) && System.Char.ToUpper(_src[_pos + 4]) == System.Char.ToUpper(c5) && System.Char.ToUpper(_src[_pos + 5]) == System.Char.ToUpper(c6))
            {
                _pos += 6;

                return true;
            }

            return false;
        }

        public bool IChar(char c1, char c2, char c3, char c4, char c5, char c6, char c7)
        {
            if (_pos + 6 < _srcLen && System.Char.ToUpper(_src[_pos]) == System.Char.ToUpper(c1) && System.Char.ToUpper(_src[_pos + 1]) == System.Char.ToUpper(c2) && System.Char.ToUpper(_src[_pos + 2]) == System.Char.ToUpper(c3) && System.Char.ToUpper(_src[_pos + 3]) == System.Char.ToUpper(c4) && System.Char.ToUpper(_src[_pos + 4]) == System.Char.ToUpper(c5) && System.Char.ToUpper(_src[_pos + 5]) == System.Char.ToUpper(c6) && System.Char.ToUpper(_src[_pos + 6]) == System.Char.ToUpper(c7))
            {
                _pos += 7;

                return true;
            }

            return false;
        }

        public bool IChar(string s)
        {
            int length = s.Length;

            if (_pos + length > _srcLen)
                return false;

            for (int i = 0; i < length; ++i)
                if (s[i] != System.Char.ToUpper(_src[_pos + i]))
                    return false;

            _pos += length;

            return true;
        }

        public bool In(char c0, char c1)
        {
            if (_pos < _srcLen && _src[_pos] >= c0 && _src[_pos] <= c1)
            {
                _pos++;

                return true;
            }

            return false;
        }

        public bool In(char c0, char c1, char c2, char c3)
        {
            if (_pos < _srcLen)
            {
                char c = _src[_pos];

                if (c >= c0 && c <= c1 || c >= c2 && c <= c3)
                {
                    _pos++;

                    return true;
                }
            }

            return false;
        }

        public bool In(char c0, char c1, char c2, char c3, char c4, char c5)
        {
            if (_pos < _srcLen)
            {
                char c = _src[_pos];

                if (c >= c0 && c <= c1 || c >= c2 && c <= c3 || c >= c4 && c <= c5)
                {
                    _pos++;

                    return true;
                }
            }

            return false;
        }

        public bool In(char c0, char c1, char c2, char c3, char c4, char c5, char c6, char c7)
        {
            if (_pos < _srcLen)
            {
                char c = _src[_pos];

                if (c >= c0 && c <= c1 || c >= c2 && c <= c3 || c >= c4 && c <= c5 || c >= c6 && c <= c7)
                {
                    _pos++;

                    return true;
                }
            }

            return false;
        }

        public bool In(string s)
        {
            if (_pos < _srcLen)
            {
                char c = _src[_pos];

                for (int i = 0; i < s.Length - 1; i += 2)
                    if (!(c >= s[i] && c <= s[i + 1]))
                        return false;

                _pos++;

                return true;
            }

            return false;
        }

        public bool Into(delMatcher toMatch, out string into)
        {
            int pos = _pos;

            if (toMatch())
            {
                into = _src.Substring(pos, _pos - pos);

                return true;
            }

            @into = "";

            return false;
        }

        public bool Into(delMatcher toMatch, out PegBegEnd begEnd)
        {
            bool matches = toMatch();

            begEnd.posBeg = _pos;
            begEnd.posEnd = _pos;

            return matches;
        }

        public bool Into(delMatcher toMatch, out int into)
        {
            string s;

            into = 0;

            if (!Into(toMatch, out s))
                return false;

            if (!System.Int32.TryParse(s, out into))
                return false;

            return true;
        }

        public bool Into(delMatcher toMatch, out double into)
        {
            string s;

            into = 0.0D;

            if (!Into(toMatch, out s))
                return false;

            if (!System.Double.TryParse(s, out into))
                return false;

            return true;
        }

        public bool IsLetter()
        {
            if (_pos < _srcLen && char.IsLetter(_src[_pos]))
            {
                _pos++;

                return true;
            }

            return false;
        }

        public static IEnumerable<PegNode> GetAncestors(PegNode pegNode)
        {
            var parent = pegNode.parent;

            if (parent != null)
            {
                yield return parent;

                foreach (var p in GetAncestors(parent))
                    yield return p;
            }
        }

        public static PegNode GetChildById(PegNode pegNode, int id)
        {
            PegNode child = pegNode.child;

            do
            {
                if (int.Equals(child.id, id))
                    return child;
            }
            while ((child = child.next) != null);

            return null;
        }

        public static PegNode GetChildByName(PegCharParser pegParser, PegNode pegNode, string name)
        {
            PegNode child = pegNode.child;

            do
            {
                if (string.Equals(pegParser.TreeNodeToString(child), name))
                    return child;
            }
            while ((child = child.next) != null);

            return null;
        }

        public static PegNode GetChildByNameUpper(PegCharParser pegParser, PegNode pegNode, string name)
        {
            PegNode child = pegNode.child;

            name = name.ToUpper();

            do
            {
                if (string.Equals(pegParser.TreeNodeToString(child).ToUpper(), name))
                    return child;
            }
            while ((child = child.next) != null);

            return null;
        }

        public static IEnumerable<PegNode> GetDescendants(PegNode pegNode)
        {
            var child = pegNode.child;

            if (child != null)
                do
                {
                    yield return child;

                    foreach (var d in GetDescendants(child))
                        yield return d;
                }
                while ((child = child.next) != null);
        }

        public static IEnumerable<PegNode> GetDescendantsAndSelf(PegNode pegNode)
        {
            yield return pegNode;

            var child = pegNode.child;

            if (child != null)
                do
                {
                    yield return child;

                    foreach (var d in GetDescendants(child))
                        yield return d;
                }
                while ((child = child.next) != null);
        }

        public static IEnumerable<PegNode> GetDescendantsById(PegNode pegNode, int id)
        {
            var child = pegNode.child;

            if (child != null)
                do
                {
                    if (int.Equals(child.id, id))
                        yield return child;

                    foreach (var d in GetDescendantsById(child, id))
                        yield return d;
                }
                while ((child = child.next) != null);
        }

        public static IEnumerable<PegNode> GetDescendantsById(PegNode pegNode, int id, int ignoreId)
        {
            var child = pegNode.child;

            if (child != null)
                do
                {
                    if (int.Equals(child.id, id)
                        && !int.Equals(child.id, ignoreId))
                        yield return child;

                    foreach (var d in GetDescendantsById(child, id, ignoreId))
                        yield return d;
                }
                while ((child = child.next) != null);
        }

        public bool NotIn(string s)
        {
            if (_pos < _srcLen)
            {
                char c = _src[_pos];

                for (int i = 0; i < s.Length - 1; i += 2)
                    if (c >= s[i] && c <= s[i + 1])
                        return false;

                _pos++;

                return true;
            }

            return false;
        }

        public bool NotOneOf(string s)
        {
            if (_pos < _srcLen)
            {
                if (s.IndexOf(_src[_pos]) == -1)
                {
                    _pos++;

                    return true;
                }
            }

            return false;
        }

        public bool OneOf(char c0, char c1)
        {
            if (_pos < _srcLen && (_src[_pos] == c0 || _src[_pos] == c1))
            {
                _pos++;

                return true;
            }

            return false;
        }

        public bool OneOf(char c0, char c1, char c2)
        {
            if (_pos < _srcLen)
            {
                char c = _src[_pos];

                if (c == c0 || c == c1 || c == c2)
                {
                    _pos++;

                    return true;
                }
            }

            return false;
        }

        public bool OneOf(char c0, char c1, char c2, char c3)
        {
            if (_pos < _srcLen)
            {
                char c = _src[_pos];

                if (c == c0 || c == c1 || c == c2 || c == c3)
                {
                    _pos++;

                    return true;
                }
            }

            return false;
        }

        public bool OneOf(char c0, char c1, char c2, char c3, char c4)
        {
            if (_pos < _srcLen)
            {
                char c = _src[_pos];

                if (c == c0 || c == c1 || c == c2 || c == c3 || c == c4)
                {
                    _pos++;

                    return true;
                }
            }

            return false;
        }

        public bool OneOf(char c0, char c1, char c2, char c3, char c4, char c5)
        {
            if (_pos < _srcLen)
            {
                char c = _src[_pos];

                if (c == c0 || c == c1 || c == c2 || c == c3 || c == c4 || c == c5)
                {
                    _pos++;

                    return true;
                }
            }

            return false;
        }

        public bool OneOf(char c0, char c1, char c2, char c3, char c4, char c5, char c6)
        {
            if (_pos < _srcLen)
            {
                char c = _src[_pos];

                if (c == c0 || c == c1 || c == c2 || c == c3 || c == c4 || c == c5 || c == c6)
                {
                    _pos++;

                    return true;
                }
            }

            return false;
        }

        public bool OneOf(char c0, char c1, char c2, char c3, char c4, char c5, char c6, char c7)
        {
            if (_pos < _srcLen)
            {
                char c = _src[_pos];

                if (c == c0 || c == c1 || c == c2 || c == c3 || c == c4 || c == c5 || c == c6 || c == c7)
                {
                    _pos++;

                    return true;
                }
            }

            return false;
        }

        public bool OneOf(string s)
        {
            if (_pos < _srcLen)
            {
                if (s.IndexOf(_src[_pos]) != -1)
                {
                    _pos++;

                    return true;
                }
            }

            return false;
        }

        public bool OneOf(OptimizedCharset cset)
        {
            if (_pos < _srcLen && cset.Matches(_src[_pos]))
            {
                _pos++;

                return true;
            }

            return false;
        }

        public bool OneOfILiterals(OptimizedLiterals litAlt)
        {
            int matchPos = _pos - 1;
            Trie node = litAlt.literalsRoot;

            for (int pos = _pos; pos < _srcLen; pos++)
            {
                char c = char.ToUpper(_src[pos]);

                if (node.children == null || c < node.min || c > node.min + node.children.Length - 1 || node.children[c - node.min] == null)
                    break;

                node = node.children[c - node.min];

                if (node.literalEnd)
                    matchPos = pos + 1;
            }

            if (matchPos >= _pos)
            {
                _pos = matchPos;

                return true;
            }

            return false;
        }

        public bool OneOfLiterals(OptimizedLiterals litAlt)
        {
            int matchPos = _pos - 1;
            Trie node = litAlt.literalsRoot;

            for (int pos = _pos; pos < _srcLen; pos++)
            {
                char c = _src[pos];

                if (node.children == null || c < node.min || c > node.min + node.children.Length - 1 || node.children[c - node.min] == null)
                    break;

                node = node.children[c - node.min];

                if (node.literalEnd)
                    matchPos = pos + 1;
            }

            if (matchPos >= _pos)
            {
                _pos = matchPos;

                return true;
            }

            return false;
        }

        public bool OptRepeat(OptimizedCharset charset)
        {
            for (; _pos < _srcLen && charset.Matches(_src[_pos]); _pos++)
            {
            }

            return true;
        }

        public bool PlusRepeat(OptimizedCharset charset)
        {
            int pos0 = _pos;

            for (; _pos < _srcLen && charset.Matches(_src[_pos]); _pos++)
            {
            }

            return _pos > pos0;
        }

        public void SetSource(string src)
        {
            ResetTree();

            if (src == null)
                src = "";

            _src = src;

            _srcLen = src.Length;
            _pos = 0;

            ResetLineStarts();
        }

        public bool Warning(string sMsg)
        {
            LogOutMsg("Warning", sMsg);

            return true;
        }

        #endregion

        #region public properties

        public string FileName { get; set; }

        #endregion

        #region constructors

        public PegCharParser()
            : this("")
        {
        }

        public PegCharParser(string src)
            : base(null)
        {
            SetSource(src);
        }

        public PegCharParser(string src, TextWriter errOut)
            : base(errOut)
        {
            SetSource(src);

            _nodeCreator = DefaultNodeCreator;
        }

        #endregion
    }
}
