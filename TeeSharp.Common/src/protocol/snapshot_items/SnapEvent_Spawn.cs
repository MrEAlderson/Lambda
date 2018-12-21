﻿using System.Runtime.InteropServices;
using TeeSharp.Common.Enums;
using TeeSharp.Common.Snapshots;

namespace TeeSharp.Common.Protocol
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public class SnapEvent_Spawn : BaseSnapEvent
    {
        public override SnapshotItems Type => SnapshotItems.EventSpawn;
    }
}