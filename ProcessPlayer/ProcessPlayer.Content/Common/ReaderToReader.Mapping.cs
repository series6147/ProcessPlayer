using ProcessPlayer.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProcessPlayer.Content.Common
{
    public class RdToRdMapping : ProcessContent
    {
        #region private variables

        private IDictionary<string, string> _fieldMapping;
        private MappingMode _mappingMode;

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
                                var res = new DataExchangeObject() { ID = ID };

                                res.Data = FieldMapping == null ? reader : new CanceledReader(new MappingDataReader(reader, FieldMapping, MappingMode), new CancellationToken[] { token });

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

        public IDictionary<string, string> FieldMapping
        {
            get { return _fieldMapping; }
            set
            {
                _fieldMapping = value;

                RaisePropertyChangedAsync("FieldMapping");
            }
        }

        public MappingMode MappingMode
        {
            get { return _mappingMode; }
            set
            {
                if (_mappingMode != value)
                {
                    _mappingMode = value;

                    RaisePropertyChangedAsync("MappingMode");
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
