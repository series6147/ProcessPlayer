using System;
using System.Threading.Tasks;

namespace ProcessPlayer.Content.Common
{
    public class ToggleSwitch : ProcessContent
    {
        #region private variables

        private bool _isChecked;

        #endregion

        #region properties

        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                if (_isChecked != value)
                {
                    _isChecked = value;

                    RaisePropertyChangedAsync("IsChecked");

                    execute();
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

            if (IsChecked)
            {
                IsExecuted = true;

                return await Task<DataExchangeObject[]>.Run(() =>
                {
                    try
                    {
                        foreach (var r in OutgoingLinks)
                            r.IncomingDataBuffer.Remove(ID);

                        var res = GetInputAsArray();

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

            return null;
        }

        #endregion
    }
}
