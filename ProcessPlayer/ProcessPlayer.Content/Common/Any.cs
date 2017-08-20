using System.Collections.Specialized;

namespace ProcessPlayer.Content.Common
{
    public class Any : ProcessContent
    {
        #region ProcessContent Members

        protected async override void OnIncomingDataBuffer_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                RaiseDataComming();

                OutputBuffer = await ExecuteAsync();
            }
        }

        #endregion
    }
}
