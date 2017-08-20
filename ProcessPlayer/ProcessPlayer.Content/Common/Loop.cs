using ProcessPlayer.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProcessPlayer.Content.Common
{
    public class Loop : ProcessContent
    {
        #region private methods

        private async Task<DataExchangeObject[]> perform()
        {
            var token = CancellationToken.Token;

            return await Task<DataExchangeObject[]>.Run(() =>
            {
                try
                {
                    if (ConditionDlg != null)
                    {
                        var res = GetInputAsArray();

                        if (res != null)
                            while (!token.IsCancellationRequested && ConditionDlg(this, Vars, Globals, new Dictionary<string, object>()).ToValue<bool>())
                            {
                                foreach (var r in OutgoingLinks)
                                    r.IncomingDataBuffer.Remove(ID);

                                foreach (var r in OutgoingLinks)
                                    r.IncomingDataBuffer[ID] = res;

                                while (OutgoingLinks.Any(t => t.IsExecuted || t.IncomingLinks.Any(r => !t.IncomingDataBuffer.ContainsKey(r.ID))))
                                    Thread.Sleep(50);
                            }
                    }
                }
                catch (Exception ex)
                {
                    Root.Log.Error(string.Format("{0} - execution\r\n{1}", ID, ex));

                    Cancel();
                }

                return null as DataExchangeObject[];
            }, token);
        }

        #endregion

        #region properties

        public string Condition { get; set; }

        public CommonDelegate ConditionDlg { get; set; }

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
                await perform();
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
                if (!string.IsNullOrEmpty(Condition))
                    ConditionDlg = BuildDelegate(Condition);
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
