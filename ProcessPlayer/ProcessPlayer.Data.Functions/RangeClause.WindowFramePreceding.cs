namespace ProcessPlayer.Data.Functions
{
    public class WindowFramePreceding
    {
        #region public methods

        public static WindowFramePreceding Create(int? amount)
        {
            return new WindowFramePreceding() { Amount = amount };
        }
        public static WindowFramePreceding Create(WindowFrame? preceding)
        {
            return new WindowFramePreceding() { Preceding = preceding };
        }
        public static WindowFramePreceding Create(int? amount, WindowFrame? preceding)
        {
            return new WindowFramePreceding() { Amount = amount, Preceding = preceding };
        }

        #endregion

        #region properties

        public int? Amount { get; set; }

        public WindowFrame? Preceding { get; set; }

        #endregion
    }
}
