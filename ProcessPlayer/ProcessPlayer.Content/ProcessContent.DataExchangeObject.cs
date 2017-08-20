namespace ProcessPlayer.Content
{
    public class DataExchangeObject
    {
        #region private variables

        private static readonly DataExchangeObject _empty = new DataExchangeObject();

        #endregion

        #region properties

        public object Data { get; set; }

        public static DataExchangeObject Empty { get { return _empty; } }

        public string ID { get; set; }

        #endregion
    }
}
