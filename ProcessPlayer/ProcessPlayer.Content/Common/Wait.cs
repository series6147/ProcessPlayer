using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProcessPlayer.Content.Common
{
    public class Wait : ProcessContent
    {
        #region private variables

        private short _waitTimeout;

        #endregion

        #region properties

        public short WaitTimeout
        {
            get { return _waitTimeout; }
            set
            {
                if (_waitTimeout != value)
                {
                    _waitTimeout = value;

                    RaisePropertyChangedAsync("WaitTimeout");
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
                    foreach (var r in OutgoingLinks)
                        r.IncomingDataBuffer.Remove(ID);

                    var res = GetInputAsArray();

                    Thread.Sleep(WaitTimeout);

                    if (token.IsCancellationRequested)
                        return null;

                    if (res != null)
                        foreach (var r in OutgoingLinks)
                            r.IncomingDataBuffer[ID] = res;

                    return res;
                }
                catch (Exception ex)
                {
                    Root.Log.Error(string.Format("{0} - execution\r\n{1}", ID, ex));

                    Cancel();
                }
                finally
                {
                    IsExecuted = false;
                }

                return null;
            }, token);
        }

        #endregion
    }
}
