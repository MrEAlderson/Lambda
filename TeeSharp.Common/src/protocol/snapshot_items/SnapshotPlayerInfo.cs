using System;
using System.Runtime.InteropServices;
using TeeSharp.Common.Enums;
using TeeSharp.Common.Snapshots;
using TeeSharp.Common.Snapshots.Extensions;

namespace TeeSharp.Common.Protocol
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public struct SnapshotPlayerInfo : ISnapshotItem
    {
        public SnapshotItems Type => SnapshotItems.PlayerInfo;
        public Span<int> Data => this.IntData();

        [MarshalAs(UnmanagedType.I4)] public PlayerFlags PlayerFlags;
        [MarshalAs(UnmanagedType.I4)] public int Score;
        [MarshalAs(UnmanagedType.I4)] public int Latency;
    }
}