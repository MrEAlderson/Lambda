namespace TeeSharp.Common.Snapshots
{
    public class SnapshotItem
    {
        public readonly int Id;
        public readonly int Key;
        public readonly ISnapshotItem Item;

        public SnapshotItem(int id, ISnapshotItem item)
        {
            Item = item;
            Key = Snapshot.Key(id, item.Type);
            Id = id;
        }
    }
}