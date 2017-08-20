using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ProcessPlayer.Content.Common
{
    public class Counter : ProcessContent
    {
        #region private variables

        private int _count;
        private int _limit;

        #endregion

        #region properties

        [JsonIgnore]
        public int Count
        {
            get { return _count; }
            set
            {
                if (_count != value)
                {
                    _count = value;

                    RaisePropertyChangedAsync("Count");
                }
            }
        }

        public int Limit
        {
            get { return _limit; }
            set
            {
                if (_limit != value)
                {
                    _limit = value;

                    RaisePropertyChangedAsync("Limit");
                }
            }
        }

        #endregion

        #region ProcessContent Members

        public override async Task<DataExchangeObject[]> ExecuteAsync()
        {
            var token = CancellationToken.Token;

            if (token.IsCancellationRequested)
                return null;

            IsExecuted = true;

            return await Task<DataExchangeObject[]>.Run(() =>
            {
                try
                {
                    var res = GetInputAsArray();

                    Root.Log.Debug(string.Format("{0} - count:{1}", ID, Count));

                    if (Count < Limit)
                    {
                        foreach (var r in OutgoingLinks.Take(1))
                            r.IncomingDataBuffer.Remove(ID);

                        if (res != null)
                            foreach (var r in OutgoingLinks.Take(1))
                                r.IncomingDataBuffer[ID] = res;
                    }
                    else
                    {
                        foreach (var r in OutgoingLinks.Skip(1).Take(1))
                            r.IncomingDataBuffer.Remove(ID);

                        if (res != null)
                            foreach (var r in OutgoingLinks.Skip(1).Take(1))
                                r.IncomingDataBuffer[ID] = res;
                    }

                    return res;
                }
                catch (Exception ex)
                {
                    Root.Log.Error(string.Format("{0} - execution\r\n{1}", ID, ex));

                    Cancel();
                }
                finally
                {
                    Count++;
                    IsExecuted = false;
                }

                return null;
            }, token);
        }

        #endregion
    }
}
