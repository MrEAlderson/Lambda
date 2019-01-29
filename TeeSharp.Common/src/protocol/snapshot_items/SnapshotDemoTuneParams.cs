﻿using System;
using System.Runtime.InteropServices;
using TeeSharp.Common.Enums;
using TeeSharp.Common.Snapshots;
using TeeSharp.Common.Snapshots.Extensions;

namespace TeeSharp.Common.Protocol
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public struct SnapshotDemoTuneParams : ISnapshotItem
    {
        public SnapshotItems Type => SnapshotItems.DemoTuneParams;
        public Span<int> Data => this.IntData();

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)] public int[] TuneParams;
    }
}