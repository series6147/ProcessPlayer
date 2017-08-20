using System.Collections;

namespace ProcessPlayer.Data.Expressions
{
    public sealed class OptimizedCharset
    {
        #region private variables

        private readonly BitArray _charSet;
        private readonly bool _negated;

        #endregion

        #region public methods

        public bool Matches(char c)
        {
            var matches = c < _charSet.Length && _charSet[c];

            if (_negated)
                return !matches;
            
            return matches;
        }

        #endregion

        #region constructors

        public OptimizedCharset(BitArray b)
            : this(b, false)
        {
        }

        public OptimizedCharset(BitArray b, bool negated)
        {
            _charSet = new BitArray(b);

            _negated = negated;
        }

        public OptimizedCharset(RangeChars[] r, char[] c)
            : this(r, c, false)
        {
        }

        public OptimizedCharset(RangeChars[] r, char[] c, bool negated)
        {
            int max = 0;

            if (r != null)
                foreach (RangeChars val in r)
                    if (val.high > max)
                        max = val.high;

            if (c != null)
                foreach (int val in c)
                    if (val > max)
                        max = val;

            _charSet = new BitArray(max + 1, false);

            if (r != null)
            {
                foreach (RangeChars val in r)
                    for (int i = val.low; i <= val.high; ++i)
                        _charSet[i] = true;
            }

            if (c != null)
                foreach (int val in c)
                    _charSet[val] = true;

            _negated = negated;
        }

        #endregion
    }
}
