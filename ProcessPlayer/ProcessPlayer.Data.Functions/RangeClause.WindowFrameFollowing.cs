namespace ProcessPlayer.Data.Functions
{
    public class WindowFrameFollowing
    {
        #region public methods

        public static WindowFrameFollowing Create(int? amount)
        {
            return new WindowFrameFollowing() { Amount = amount };
        }
        public static WindowFrameFollowing Create(WindowFrame? preceding)
        {
            return new WindowFrameFollowing() { Preceding = preceding };
        }
        public static WindowFrameFollowing Create(int? amount, WindowFrame? preceding)
        {
            return new WindowFrameFollowing() { Amount = amount, Preceding = preceding };
        }

        #endregion

        #region properties

        public int? Amount { get; set; }

        public WindowFrame? Preceding { get; set; }

        #endregion
    }
}
