using System;
using System.Threading.Tasks;

namespace ProcessPlayer.Content.Common
{
    public class Copier : ProcessContent
    {
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

                    foreach (var r in OutgoingLinks)
                    {
                        foreach (var input in IncomingDataBuffer)
                            r.IncomingDataBuffer[input.Key] = input.Value;

                        r.IncomingDataBuffer[ID] = new DataExchangeObject[] { };
                    }
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

                return new DataExchangeObject[] { };
            }, token);
        }

        #endregion
    }
}
