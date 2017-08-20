namespace ProcessPlayer.Data.Expressions
{
    public enum AddPolicy
    {
        eAddAsChild,
        eAddAsSibling
    };

    public struct PegBegEnd
    {
        #region public variables

        public int posBeg, posEnd;

        #endregion

        #region public methods

        public string GetAsString(string src)
        {
            return src.Substring(posBeg, Length);
        }

        #endregion

        #region properties

        public int Length
        {
            get { return posEnd - posBeg; }
        }

        #endregion
    }

    public struct PegTree
    {
        public AddPolicy addPolicy;
        public PegNode cur, root;
    }

    public struct RangeBytes
    {
        #region public variables

        public byte high, low;

        #endregion

        #region constructors

        public RangeBytes(byte low, byte high)
        {
            this.high = high;
            this.low = low;
        }

        #endregion
    }

    public struct RangeChars
    {
        #region public variables

        public char high, low;

        #endregion

        #region constructors

        public RangeChars(char low, char high)
        {
            this.high = high;
            this.low = low;
        }

        #endregion
    }
}
