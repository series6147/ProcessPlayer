using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProcessPlayer.Content.Common
{
    public class Collector : ProcessContent
    {
        #region private methods

        private Task<DataExchangeObject[]> perform()
        {
            var token = CancellationToken.Token;

            return Task<DataExchangeObject[]>.Factory.StartNew(() =>
            {
                while (!token.IsCancellationRequested)
                {
                    if (IncomingLinks.All(r => IncomingDataBuffer.ContainsKey(r.ID)))
                        break;

                    Thread.Sleep(50);
                }

                DataExchangeObject[] res;

                lock (IncomingDataBuffer.SyncContext)
                {
                    res = IncomingDataBuffer.Values.SelectMany(v => v).ToArray();
                }

                return res;
            }, token);
        }

        #endregion

        #region ProcessContent Members

        public override async Task<DataExchangeObject[]> ExecuteAsync()
        {
            var token = CancellationToken.Token;

            if (token.IsCancellationRequested)
                return null;

            IsExecuted = true;

            try
            {
                foreach (var r in OutgoingLinks)
                    r.IncomingDataBuffer.Remove(ID);

                var res = await perform();

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
        }

        public override void RaiseDataComming()
        {
        }

        public override void RaiseExecuteFinished()
        {
        }

        public override void RaiseExecuteStarted()
        {
        }

        #endregion
    }
}
