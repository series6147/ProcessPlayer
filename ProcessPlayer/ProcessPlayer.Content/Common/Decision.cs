using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProcessPlayer.Content.Common
{
    public class Decision : ProcessContent
    {
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

            if (ConditionDlg != null)
            {
                IsExecuted = true;

                return await Task<DataExchangeObject[]>.Run(() =>
                {
                    try
                    {
                        var ids = ConditionDlg(this, Vars, Globals, IncomingDataBuffer.ToDictionarySync());

                        if (ids is IEnumerable<object>)
                        {
                            var outgoingLinks = getContentsByIDs(((IEnumerable<object>)ids).Cast<string>());

                            foreach (var r in outgoingLinks)
                                r.IncomingDataBuffer.Remove(ID);

                            var res = GetInputAsArray();

                            if (res != null)
                                foreach (var r in outgoingLinks)
                                    r.IncomingDataBuffer[ID] = res;

                            return res;
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

                    return null;
                }, token);
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
