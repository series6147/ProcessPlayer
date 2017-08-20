using ProcessPlayer.Data.Expressions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProcessPlayer.Data.CodeGen.Generators
{
    public abstract class CodeGen
    {
        #region protected variables

        protected Dictionary<int, Action<PegNode, StringBuilder, int>> _actions;
        protected IDictionary<string, int> _properties;
        protected PegCharParser _parser;
        protected string _expression;

        #endregion

        #region protected methods

        protected void DefaultNodeGen(PegNode node, StringBuilder sb, int spaceCount, bool brackets)
        {
            if (sb.Length > 0 && (char.Equals(sb[sb.Length - 1], '\n') || char.Equals(sb[sb.Length - 1], '\r')))
                sb.Append(string.Empty.PadRight(IndentSize * spaceCount, IndentChar));

            if (brackets)
                sb.Append('(');

            foreach (var n in GetNodeChildren(node))
            {
                if (Enum.IsDefined(EnumType, n.id))
                    _actions[n.id](n, sb, spaceCount);
                else
                    sb.Append(n.GetAsString(_expression));
            }

            if (brackets)
                sb.Append(')');
        }

        protected IEnumerable<PegNode> GetNodeChildren(PegNode node)
        {
            if (node != null && (node = node.child) != null)
                do
                {
                    yield return node;
                }
                while ((node = node.next) != null);
        }

        protected string GetNodeString(PegNode node)
        {
            var sb = new StringBuilder();

            if (Enum.IsDefined(EnumType, node.id))
                _actions[node.id](node, sb, 0);
            else
                sb.Append(node.GetAsString(_expression));

            return sb.ToString();
        }

        protected IList<string> ToList(IEnumerable<PegNode> children)
        {
            var prms = new List<string>();
            var sbPrms = new StringBuilder();

            foreach (var child in children)
            {
                sbPrms.Remove(0, sbPrms.Length);

                if (Enum.IsDefined(EnumType, child.id))
                    _actions[child.id](child, sbPrms, 0);
                else
                    sbPrms.Append(child.GetAsString(_expression));

                prms.Add(sbPrms.ToString());
            }

            return prms;
        }

        protected void TruncateOrTerminateLine(StringBuilder sb)
        {
            var i = sb.Length - 1;
            var term = false;

            for (; i >= 0; i--)
            {
                if (char.Equals(sb[i], '\n') || char.Equals(sb[i], '\r'))
                    break;
                else if (char.Equals(sb[i], ' ') || char.Equals(sb[i], '\t'))
                    continue;

                term = true;

                break;
            }

            if (sb.Length > 0)
            {
                if (term)
                    sb.AppendLine();
                else
                    sb.Length = i + 1;
            }
        }

        #endregion

        #region public methods

        public abstract void Generate(string src, StringBuilder sb, IDictionary<string, int> properties, char indentChar, int indentSize);

        #endregion

        #region properties

        protected virtual Type EnumType
        {
            get { return typeof(EConditionalParser); }
        }

        public char IndentChar { get; set; }

        public int IndentSize { get; set; }

        #endregion

        #region constructors

        protected CodeGen()
        {
            IndentChar = ' ';
            IndentSize = 4;
        }

        #endregion
    }
}
