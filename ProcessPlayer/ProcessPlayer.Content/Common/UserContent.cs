using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProcessPlayer.Content.Common
{
    public class UserContent : ProcessContent
    {
        #region private methods

        private async Task<DataExchangeObject[]> perform()
        {
            var token = CancellationToken.Token;

            return await Task<DataExchangeObject[]>.Run(() =>
            {
                try
                {
                    var res = PerformDlg == null ? null : PerformDlg(this, Vars, Globals, new Dictionary<string, object>());

                    if (res is DataExchangeObject)
                        return new DataExchangeObject[] { (DataExchangeObject)res };
                    else if (res is DataExchangeObject[])
                        return (DataExchangeObject[])res;
                    else
                        return new DataExchangeObject[] { new DataExchangeObject() { Data = res } };
                }
                catch (Exception ex)
                {
                    Root.Log.Error(string.Format("{0} - execution\r\n{1}", ID, ex));

                    Cancel();
                }

                return null;
            }, token);
        }

        #endregion

        #region properties

        public string Perform { get; set; }

        public CommonDelegate PerformDlg { get; set; }

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

        public override async Task Initialize()
        {
            try
            {
                if (!string.IsNullOrEmpty(Perform))
                    PerformDlg = BuildDelegate(Perform);
            }
            catch (Exception ex)
            {
                Root.Log.Error(string.Format("{0} - {1}\r\n{2}", ID, _Initialization, ex));

                Cancel();
            }

            await base.Initialize();
        }

        #endregion
    }
}
