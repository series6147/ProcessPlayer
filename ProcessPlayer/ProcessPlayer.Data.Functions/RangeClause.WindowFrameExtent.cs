namespace ProcessPlayer.Data.Functions
{
    public class WindowFrameExtent
    {
        #region public methods

        public static WindowFrameExtent Create(WindowFrameBetween between)
        {
            return new WindowFrameExtent() { Between = between };
        }

        public static WindowFrameExtent Create(WindowFramePreceding preceding)
        {
            return new WindowFrameExtent() { Preceding = preceding };
        }

        #endregion

        #region properties

        public WindowFrameBetween Between { get; set; }

        public WindowFramePreceding Preceding { get; set; }

        #endregion
    }
}
