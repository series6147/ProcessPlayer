using ProcessPlayer.Content.Models;
using ProcessPlayer.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProcessPlayer.Content.Common
{
    public class RdToRdCalculation : ProcessContent
    {
        #region private variables

        private IEnumerable<Field> _executionFields;

        #endregion

        #region private methods

        private async Task<DataExchangeObject[]> perform()
        {
            var token = CancellationToken.Token;

            return Allowed()
                ? await Task<DataExchangeObject[]>.Factory.StartNew(() =>
                {
                    var resultSet = new List<DataExchangeObject>();

                    try
                    {
                        foreach (var inc in GetInputAsArray())
                        {
                            if (token.IsCancellationRequested)
                                break;

                            var reader = inc.Data.ToDataReader();

                            if (reader != null)
                            {
                                var readerFields = new HashSet<string>(reader.GetFieldNames());
                                var res = new DataExchangeObject() { ID = ID };

                                if (ExecutionFields != null && ExecutionFields.Any())
                                {
                                }
                                else
                                    res.Data = reader;

                                resultSet.Add(res);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Root.Log.Error(string.Format("{0} - execution\r\n{1}", ID, ex));

                        Cancel();
                    }

                    return resultSet.ToArray();
                }, token)
                : null;
        }

        #endregion

        #region properties

        public IEnumerable<Field> ExecutionFields
        {
            get { return _executionFields; }
            set
            {
                _executionFields = value;

                RaisePropertyChangedAsync("ExecutionFields");
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

        #endregion
    }
}
