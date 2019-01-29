using System;
using System.Runtime.InteropServices;
using TeeSharp.Common.Enums;
using TeeSharp.Common.Snapshots;
using TeeSharp.Common.Snapshots.Extensions;

namespace TeeSharp.Common.Protocol
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public struct SnapshotGameDataFlag  : ISnapshotItem
    {
        public SnapshotItems Type => SnapshotItems.GameDataFlag;
        public Span<int> Data => this.IntData();

        [MarshalAs(UnmanagedType.I4)] public int FlagCarrierRed;
        [MarshalAs(UnmanagedType.I4)] public int FlagCarrierBlue;
        [MarshalAs(UnmanagedType.I4)] public int FlagDropTickRed;
        [MarshalAs(UnmanagedType.I4)] public int FlagDropTickBlue;
    }
}