using System;
using System.Runtime.InteropServices;
using TeeSharp.Common.Enums;
using TeeSharp.Common.Snapshots;
using TeeSharp.Common.Snapshots.Extensions;

namespace TeeSharp.Common.Protocol
{
    public struct SnapshotCharacter : ISnapshotItem, IEquatable<SnapshotCharacter>
    {
        public SnapshotItems Type => SnapshotItems.Character;
        public Span<int> Data => this.IntData();

        public SnapshotCharacterCore CharacterCore;
        [MarshalAs(UnmanagedType.I4)] public int Health;
        [MarshalAs(UnmanagedType.I4)] public int Armor;
        [MarshalAs(UnmanagedType.I4)] public int AmmoCount;
        [MarshalAs(UnmanagedType.I4)] public Weapon Weapon;
        [MarshalAs(UnmanagedType.I4)] public Emote Emote;
        [MarshalAs(UnmanagedType.I4)] public int AttackTick;
        [MarshalAs(UnmanagedType.I4)] public CoreEvents TriggeredEvents;

        public bool Equals(SnapshotCharacter other)
        {
            return CharacterCore.Equals(other.CharacterCore) && Health == other.Health && Armor == other.Armor &&
                   AmmoCount == other.AmmoCount && Weapon == other.Weapon && Emote == other.Emote &&
                   AttackTick == other.AttackTick && TriggeredEvents == other.TriggeredEvents;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is SnapshotCharacter other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = CharacterCore.GetHashCode();
                hashCode = (hashCode * 397) ^ Health;
                hashCode = (hashCode * 397) ^ Armor;
                hashCode = (hashCode * 397) ^ AmmoCount;
                hashCode = (hashCode * 397) ^ (int) Weapon;
                hashCode = (hashCode * 397) ^ (int) Emote;
                hashCode = (hashCode * 397) ^ AttackTick;
                hashCode = (hashCode * 397) ^ (int) TriggeredEvents;
                return hashCode;
            }
        }
    }
}