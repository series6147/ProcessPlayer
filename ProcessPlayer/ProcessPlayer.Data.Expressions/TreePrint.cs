using System.IO;

namespace ProcessPlayer.Data.Expressions
{
    public class TreePrint : PrintNode
    {
        #region private variables

        private readonly bool _verbose;
        private readonly int _maxLineLen;
        private readonly delGetNodeName _getNodeName;
        private readonly string _src;
        private readonly TextWriter _treeOut;

        #endregion

        #region overriden methods

        public override bool IsLeaf(PegNode node)
        {
            return node.child == null;
        }

        public override int LenDistNext(PegNode node, bool alignVertical, ref int offsetLineBeg, int level)
        {
            return 1;
        }

        public override int LenLeaf(PegNode node)
        {
            int nLen = node.match.posEnd - node.match.posBeg + 2;

            if (_verbose)
                nLen += LenIdAsName(node) + 2;

            return nLen;
        }

        public override int LenMaxLine()
        {
            return _maxLineLen;
        }

        public override int LenNodeBeg(PegNode node)
        {
            return LenIdAsName(node) + 1;
        }

        public override int LenNodeEnd(PegNode node)
        {
            return 1;
        }

        public override void PrintDistNext(PegNode node, bool alignVertical, ref int offsetLineBeg, int level)
        {
            if (alignVertical)
            {
                _treeOut.WriteLine();
                _treeOut.Write(new string(' ', offsetLineBeg));
            }
            else
            {
                _treeOut.Write(' ');

                offsetLineBeg++;
            }
        }

        public override void PrintLeaf(PegNode node, ref int offsetLineBeg, bool alignVertical)
        {
            if (_verbose)
            {
                PrintIdAsName(node);

                _treeOut.Write('<');
            }

            int len = node.match.posEnd - node.match.posBeg;

            _treeOut.Write("'");

            if (len > 0)
                _treeOut.Write(_src.Substring(node.match.posBeg, node.match.posEnd - node.match.posBeg));

            _treeOut.Write("'");

            if (_verbose)
                _treeOut.Write('>');
        }

        public override void PrintNodeBeg(PegNode node, bool alignVertical, ref int offsetLineBeg, int level)
        {
            PrintIdAsName(node);

            _treeOut.Write("<");

            if (alignVertical)
            {
                _treeOut.WriteLine();
                _treeOut.Write(new string(' ', offsetLineBeg += 2));
            }
            else
                offsetLineBeg++;
        }

        public override void PrintNodeEnd(PegNode node, bool alignVertical, ref int offsetLineBeg, int level)
        {
            if (alignVertical)
            {
                _treeOut.WriteLine();
                _treeOut.Write(new string(' ', offsetLineBeg -= 2));
            }

            _treeOut.Write('>');

            if (!alignVertical)
                offsetLineBeg++;
        }

        #endregion

        #region private methods

        private int DetermineLineLength(PegNode parent, int offsetLineBeg)
        {
            int len = LenNodeBeg(parent);
            PegNode node;

            for (node = parent.child; node != null; node = node.next)
            {
                if (IsSkip(node))
                    continue;

                if (IsLeaf(node))
                    len += LenLeaf(node);
                else
                    len += DetermineLineLength(node, offsetLineBeg);

                if (len + offsetLineBeg > LenMaxLine())
                    return len + offsetLineBeg;
            }

            len += LenNodeEnd(node);

            return len;
        }

        private int LenIdAsName(PegNode node)
        {
            string name = _getNodeName(node);

            return name.Length;
        }

        private void PrintIdAsName(PegNode node)
        {
            string name = _getNodeName(node);

            _treeOut.Write(name);
        }

        #endregion

        #region public methods

        public void PrintTree(PegNode parent, int offsetLineBeg, int level)
        {
            if (IsLeaf(parent))
            {
                PrintLeaf(parent, ref offsetLineBeg, false);

                _treeOut.Flush();

                return;
            }

            bool alignVertical =
                DetermineLineLength(parent, offsetLineBeg) > LenMaxLine();

            PrintNodeBeg(parent, alignVertical, ref offsetLineBeg, level);

            int nOffset = offsetLineBeg;

            for (PegNode node = parent.child; node != null; node = node.next)
            {
                if (IsSkip(node))
                    continue;

                if (IsLeaf(node))
                    PrintLeaf(node, ref offsetLineBeg, alignVertical);
                else
                    PrintTree(node, offsetLineBeg, level + 1);

                if (alignVertical)
                    offsetLineBeg = nOffset;

                while (node.next != null && IsSkip(node.next))
                    node = node.next;

                if (node.next != null)
                    PrintDistNext(node, alignVertical, ref offsetLineBeg, level);
            }

            PrintNodeEnd(parent, alignVertical, ref  offsetLineBeg, level);

            _treeOut.Flush();
        }

        #endregion

        #region constructors

        public TreePrint(TextWriter treeOut, string src, int maxLineLen, delGetNodeName getNodeName, bool verbose)
        {
            _getNodeName = getNodeName;
            _maxLineLen = maxLineLen;
            _src = src;
            _treeOut = treeOut;
            _verbose = verbose;
        }

        #endregion
    }
}
