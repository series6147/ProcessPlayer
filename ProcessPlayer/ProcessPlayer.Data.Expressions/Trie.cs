using System.Collections.Generic;
using System.Linq;

namespace ProcessPlayer.Data.Expressions
{
    internal class Trie
    {
        #region fields

        internal bool literalEnd;   // end of literal
        internal char min;          // first valid character in children
        internal char charThis;     // character stored in this node
        internal Trie[] children;   // contains the successor node of cThis_;

        #endregion

        #region internal methods

        internal Trie(char charThis, int index, string[] literals)
        {
            char max = char.MinValue;

            this.charThis = charThis;
            min = char.MaxValue;

            var followChars = new Dictionary<char, byte>();

            foreach (string literal in literals)
            {
                if (literal == null || index > literal.Length)
                    continue;

                if (index == literal.Length)
                {
                    literalEnd = true;

                    continue;
                }

                char c = literal[index];

                if (!followChars.ContainsKey(c))
                    followChars.Add(c, 0);

                if (c < min)
                    min = c;

                if (c > max)
                    max = c;
            }

            if (followChars.Count == 0)
                children = null;
            else
            {
                children = new Trie[(max - min) + 1];

                foreach (var kvp in followChars)
                {
                    var c = kvp.Key;

                    children[c - min] = new Trie(c, index + 1, literals.Where(s => index < s.Length).Where(s => c == s[index]).ToArray());
                }
            }
        }

        #endregion
    }
}
