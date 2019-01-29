using System;
using TeeSharp.Common.Enums;

namespace TeeSharp.Common.Snapshots
{
    public interface ISnapshotItem
    {
        SnapshotItems Type { get; }
        Span<int> Data { get; }
    }
}