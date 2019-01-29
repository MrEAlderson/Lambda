namespace TeeSharp.Common.Snapshots
{
    public class SnapshotItemWrapper<T> where T : struct, ISnapshotItem
    {
        public ISnapshotItem Item { get; set; }
    }
}