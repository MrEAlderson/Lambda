using TeeSharp.Common.Protocol;

namespace TeeSharp.Common.Snapshots
{
    public interface ISnapshotEvent : ISnapshotItem
    {
         SnapshotEventCommon Base { get; }
    }
}