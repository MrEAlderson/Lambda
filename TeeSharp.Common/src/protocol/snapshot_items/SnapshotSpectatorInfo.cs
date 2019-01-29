using System;
using System.Runtime.InteropServices;
using TeeSharp.Common.Enums;
using TeeSharp.Common.Snapshots;
using TeeSharp.Common.Snapshots.Extensions;

namespace TeeSharp.Common.Protocol
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public struct SnapshotSpectatorInfo : ISnapshotItem
    {
        public SnapshotItems Type => SnapshotItems.SpectatorInfo;
        public Span<int> Data => this.IntData();

        [MarshalAs(UnmanagedType.I4)] public SpectatorMode SpectatorMode;
        [MarshalAs(UnmanagedType.I4)] public int SpectatorId;
        [MarshalAs(UnmanagedType.I4)] public int X;
        [MarshalAs(UnmanagedType.I4)] public int Y;
    }
}