using ProcessPlayer.Content;
using ProcessPlayer.Content.Common;
using ProcessPlayer.Data.Common;
using System;
using System.Threading.Tasks;

namespace ProcessPlayer.Content.Common
{
    public class Selector : ProcessContent
    {
        #region private variables

        private bool _executeEnabled;
        private object _parameter;
        private RelayCommand _command;

        #endregion

        #region private methods

        private async void execute(object parameter)
        {
            _executeEnabled = true;
            _parameter = parameter;

            OutputBuffer = await ExecuteAsync();
        }

        #endregion

        #region commands

        public RelayCommand Command
        {
            get
            {
                if (_command == null)
                    _command = new RelayCommand(execute);
                return _command;
            }
        }

        #endregion

        #region ProcessContent Members

        public override async Task<DataExchangeObject[]> ExecuteAsync()
        {
            var token = CancellationToken.Token;

            if (token.IsCancellationRequested || !_executeEnabled)
                return null;

            _executeEnabled = false;

            IsExecuted = true;

            return await Task<DataExchangeObject[]>.Run(() =>
            {
                try
                {
                    var outgoingLinks = getContentsByIDs(new string[] { _parameter as string });

                    foreach (var r in outgoingLinks)
                        r.IncomingDataBuffer.Remove(ID);

                    var res = GetInputAsArray();

                    if (res != null)
                        foreach (var r in outgoingLinks)
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
