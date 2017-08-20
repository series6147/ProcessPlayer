using ProcessPlayer.Data.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProcessPlayer.Content.Common
{
    public class RdToRdSchema : ProcessContent
    {
        #region private variables

        private bool _multipleSchema;

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
                            if (token.IsCancellationRequested || Root.CancellationToken.IsCancellationRequested)
                                break;

                            var reader = inc.Data.ToDataReader();

                            if (reader != null)
                            {
                                var res = new DataExchangeObject() { ID = ID };
                                var readers = new List<IDataReader>();

                                do
                                {
                                    var fields = Enumerable.Range(0, reader.FieldCount).ToDictionary(i => reader.GetName(i), i => reader.GetFieldType(i));

                                    readers.Add(new DictionaryReader<string, Type>(fields, new[] { "name", "type" }));
                                } while (!token.IsCancellationRequested && !Root.CancellationToken.IsCancellationRequested && MultipleSchema && reader.NextResult());

                                if (!token.IsCancellationRequested && !Root.CancellationToken.IsCancellationRequested)
                                {
                                    res.Data = new CanceledReader(new PostProcessDataReader(new UnionReader(readers)
                                        , new Dictionary<string, Func<object[], object>>() {
                                        { "type", (values) => {
                                            if ((values[1] as Type).IsDateTime())
                                                return "Date";
                                            if ((values[1] as Type).IsNumeric())
                                                return "Number";
                                            return "String";
                                        } } }), new CancellationToken[] { token });

                                    resultSet.Add(res);
                                }
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

        public bool MultipleSchema
        {
            get { return _multipleSchema; }
            set
            {
                if (_multipleSchema != value)
                {
                    _multipleSchema = value;

                    RaisePropertyChangedAsync("MultipleSchema");
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
