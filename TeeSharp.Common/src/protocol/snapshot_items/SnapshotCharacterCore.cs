using System;
using System.Runtime.InteropServices;
using TeeSharp.Common.Enums;
using TeeSharp.Common.Snapshots;
using TeeSharp.Common.Snapshots.Extensions;

namespace TeeSharp.Common.Protocol
{
    public struct SnapshotCharacterCore : ISnapshotItem, IEquatable<SnapshotCharacterCore>
    {
        public SnapshotItems Type => SnapshotItems.CharacterCore;
        public Span<int> Data => this.IntData();

        [MarshalAs(UnmanagedType.I4)] public int Tick;
        [MarshalAs(UnmanagedType.I4)] public int X;
        [MarshalAs(UnmanagedType.I4)] public int Y;
        [MarshalAs(UnmanagedType.I4)] public int VelX;
        [MarshalAs(UnmanagedType.I4)] public int VelY;
        [MarshalAs(UnmanagedType.I4)] public int Angle;
        [MarshalAs(UnmanagedType.I4)] public int Direction;
        [MarshalAs(UnmanagedType.I4)] public int Jumped;
        [MarshalAs(UnmanagedType.I4)] public int HookedPlayer;
        [MarshalAs(UnmanagedType.I4)] public HookState HookState;
        [MarshalAs(UnmanagedType.I4)] public int HookTick;
        [MarshalAs(UnmanagedType.I4)] public int HookX;
        [MarshalAs(UnmanagedType.I4)] public int HookY;
        [MarshalAs(UnmanagedType.I4)] public int HookDx;
        [MarshalAs(UnmanagedType.I4)] public int HookDy;

        public bool Equals(SnapshotCharacterCore other)
        {
            return Tick == other.Tick && X == other.X && Y == other.Y && VelX == other.VelX && VelY == other.VelY &&
                   Angle == other.Angle && Direction == other.Direction && Jumped == other.Jumped &&
                   HookedPlayer == other.HookedPlayer && HookState == other.HookState && HookTick == other.HookTick &&
                   HookX == other.HookX && HookY == other.HookY && HookDx == other.HookDx && HookDy == other.HookDy;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is SnapshotCharacterCore other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Tick;
                hashCode = (hashCode * 397) ^ X;
                hashCode = (hashCode * 397) ^ Y;
                hashCode = (hashCode * 397) ^ VelX;
                hashCode = (hashCode * 397) ^ VelY;
                hashCode = (hashCode * 397) ^ Angle;
                hashCode = (hashCode * 397) ^ Direction;
                hashCode = (hashCode * 397) ^ Jumped;
                hashCode = (hashCode * 397) ^ HookedPlayer;
                hashCode = (hashCode * 397) ^ (int) HookState;
                hashCode = (hashCode * 397) ^ HookTick;
                hashCode = (hashCode * 397) ^ HookX;
                hashCode = (hashCode * 397) ^ HookY;
                hashCode = (hashCode * 397) ^ HookDx;
                hashCode = (hashCode * 397) ^ HookDy;
                return hashCode;
            }
        }
    }
}