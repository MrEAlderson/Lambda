using System;
using System.Runtime.InteropServices;
using TeeSharp.Common.Enums;
using TeeSharp.Common.Snapshots;
using TeeSharp.Common.Snapshots.Extensions;

namespace TeeSharp.Common.Protocol
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public struct SnapshotGameData : ISnapshotItem
    {
        public SnapshotItems Type => SnapshotItems.GameData;
        public Span<int> Data => this.IntData();

        [MarshalAs(UnmanagedType.I4)] public int GameStartTick;
        [MarshalAs(UnmanagedType.I4)] public GameStateFlags GameStateFlags;
        [MarshalAs(UnmanagedType.I4)] public int GameStateEndTick;
    }
}