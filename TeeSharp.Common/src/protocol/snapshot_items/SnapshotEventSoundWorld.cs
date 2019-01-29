using System;
using System.Runtime.InteropServices;
using TeeSharp.Common.Enums;
using TeeSharp.Common.Snapshots;
using TeeSharp.Common.Snapshots.Extensions;

namespace TeeSharp.Common.Protocol
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public struct SnapshotEventSoundWorld : ISnapshotEvent
    {
        public SnapshotItems Type => SnapshotItems.EventSoundWorld;
        public Span<int> Data => this.IntData();

        public SnapshotEventCommon Base => Common;
        public SnapshotEventCommon Common;
        [MarshalAs(UnmanagedType.I4)] public Sound Sound;
    }
}