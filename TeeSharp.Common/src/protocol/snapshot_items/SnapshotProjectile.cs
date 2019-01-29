using System;
using System.Runtime.InteropServices;
using TeeSharp.Common.Enums;
using TeeSharp.Common.Snapshots;
using TeeSharp.Common.Snapshots.Extensions;

namespace TeeSharp.Common.Protocol
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public struct SnapshotProjectile : ISnapshotItem
    {
        public SnapshotItems Type => SnapshotItems.Projectile;
        public Span<int> Data => this.IntData();

        [MarshalAs(UnmanagedType.I4)] public int X;
        [MarshalAs(UnmanagedType.I4)] public int Y;
        [MarshalAs(UnmanagedType.I4)] public int VelX;
        [MarshalAs(UnmanagedType.I4)] public int VelY;
        [MarshalAs(UnmanagedType.I4)] public Weapon Weapon;
        [MarshalAs(UnmanagedType.I4)] public int StartTick;
    }
}