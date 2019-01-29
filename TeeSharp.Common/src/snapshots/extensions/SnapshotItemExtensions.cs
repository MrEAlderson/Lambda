using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace TeeSharp.Common.Snapshots.Extensions
{
    public static class SnapshotItemExtensions
    {
        public static T FromData<T>(this ReadOnlySpan<int> data) where T : struct, ISnapshotItem
        {
            return MemoryMarshal.Read<T>(MemoryMarshal.AsBytes(data));
        }

        public static Span<int> IntData<T>(this T item) where T : struct, ISnapshotItem
        {
            Span<byte> span = new byte[Unsafe.SizeOf<T>()];
            MemoryMarshal.Write(span, ref item);
            return MemoryMarshal.Cast<byte, int>(span);
        }
    }
}