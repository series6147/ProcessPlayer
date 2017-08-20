namespace ProcessPlayer.Data.Functions
{
    public class WindowFrameBetween
    {
        #region public methods

        public static WindowFrameBetween Create(WindowFrameBound start, WindowFrameBound end)
        {
            return new WindowFrameBetween() { End = end, Start = start };
        }

        #endregion

        #region properties

        public WindowFrameBound End { get; set; }

        public WindowFrameBound Start { get; set; }

        #endregion
    }
}
