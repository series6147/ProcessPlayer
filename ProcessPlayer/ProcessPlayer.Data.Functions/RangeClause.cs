namespace ProcessPlayer.Data.Functions
{
    public class RangeClause
    {
        #region public methods

        public static RangeClause Create(WindowFrameExtent extent)
        {
            return new RangeClause() { Extent = extent };
        }

        #endregion

        #region properties

        public WindowFrameExtent Extent { get; set; }

        #endregion
    }
}
