namespace ProcessPlayer.Data.Functions
{
    public class WindowFrameBound
    {
        #region public methods

        public static WindowFrameBound Create(WindowFramePreceding following, WindowFramePreceding preceding)
        {
            return new WindowFrameBound() { Following = following, Preceding = preceding };
        }

        #endregion

        #region properties

        public WindowFramePreceding Following { get; set; }

        public WindowFramePreceding Preceding { get; set; }

        #endregion
    }
}
