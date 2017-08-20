using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProcessPlayer.Content.Common
{
    public class Group : ProcessContent
    {
        #region private variables

        private bool _isIsolated;
        private Collector _collector;

        #endregion

        #region private methods

        private async Task<DataExchangeObject[]> perform()
        {
            try
            {
                var token = CancellationToken;

                if (token.IsCancellationRequested)
                    return null;

                var incomingDataBuffer = (ObservableDictionary<string, DataExchangeObject[]>)IncomingDataBuffer.Clone();

                Collector.CancellationToken = token;
                Collector.IncomingDataBuffer.Clear();

                foreach (ProcessContent r in Children.Where(r => r.IncomingLinks == null || !r.IncomingLinks.Any()).ToArray())
                {
                    r.IncomingDataBuffer = incomingDataBuffer;
                    r.ExecuteAsync();

                    if (token.IsCancellationRequested)
                        return null;
                }

                return await Collector.ExecuteAsync();
            }
            catch (Exception ex)
            {
                Root.Log.Error(string.Format("{0} - execution\r\n{1}", ID, ex));

                Cancel();
            }

            return null;
        }

        #endregion

        #region properties

        public override CancellationTokenSource CancellationToken
        {
            get
            {
                if (IsIsolated)
                {
                    if (_cancellationToken == null)
                    {
                        _cancellationToken = new CancellationTokenSource();

                        if (_collector != null)
                            _collector.CancellationToken = _cancellationToken;
                    }
                    return _cancellationToken;
                }
                return base.CancellationToken;
            }
            set
            {
                if (IsIsolated)
                {
                    _cancellationToken = value;

                    if (_collector != null)
                        _collector.CancellationToken = _cancellationToken;
                }
                else
                    base.CancellationToken = value;
            }
        }

        private Collector Collector
        {
            get
            {
                if (_collector == null)
                    _collector = new Collector() { CancellationToken = CancellationToken, Root = Root };
                return _collector;
            }
        }

        public bool IsIsolated
        {
            get { return _isIsolated; }
            set
            {
                if (_isIsolated != value)
                {
                    _isIsolated = value;

                    RaisePropertyChangedAsync("IsIsolated");
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

        public override async Task Initialize()
        {
            var token = CancellationToken.Token;

            if (token.IsCancellationRequested)
                return;

            await Task.Run((Action)(() =>
            {
                var processes = Children.Where(r => r.OutgoingLinks == null || !r.OutgoingLinks.Any()).ToArray();

                if (!processes.Any())
                    throw new Exception("No the ending content.");

                Collector.IncomingLinks = processes;

                foreach (var r in processes)
                    r.OutgoingLinks = new ProcessContent[] { Collector };

            }), token);

            await base.Initialize();
        }

        #endregion
    }
}
