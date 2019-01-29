using System;
using System.Runtime.InteropServices;
using TeeSharp.Common.Enums;
using TeeSharp.Common.Snapshots;
using TeeSharp.Common.Snapshots.Extensions;

namespace TeeSharp.Common.Protocol
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public struct SnapshotDemoGameInfo : ISnapshotItem
    {
        public SnapshotItems Type => SnapshotItems.DemoGameInfo;
        public Span<int> Data => this.IntData();

        [MarshalAs(UnmanagedType.I4)] public GameFlags GameFlags;
        [MarshalAs(UnmanagedType.I4)] public int ScoreLimit;
        [MarshalAs(UnmanagedType.I4)] public int TimeLimit;
        [MarshalAs(UnmanagedType.I4)] public int MatchNum;
        [MarshalAs(UnmanagedType.I4)] public int MatchCurrent;
    }
}