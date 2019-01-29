using System;
using System.Runtime.InteropServices;
using TeeSharp.Common.Enums;
using TeeSharp.Common.Snapshots;
using TeeSharp.Common.Snapshots.Extensions;

namespace TeeSharp.Common.Protocol
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public struct SnapshotEventDamage : ISnapshotEvent
    {
        public SnapshotItems Type => SnapshotItems.EventDamage;
        public Span<int> Data => this.IntData();

        public bool IsSelf
        {
            get => Self != 0;
            set => Self = value ? 1 : 0;
        }

        public SnapshotEventCommon Base => Common;
        public SnapshotEventCommon Common;
        [MarshalAs(UnmanagedType.I4)] public int ClientId;
        [MarshalAs(UnmanagedType.I4)] public int Angle;
        [MarshalAs(UnmanagedType.I4)] public int HealthAmount;
        [MarshalAs(UnmanagedType.I4)] public int ArmorAmount;
        [MarshalAs(UnmanagedType.I4)] public int Self;
    }
}