﻿using TeeSharp.Common.Enums;
using TeeSharp.Network;

namespace TeeSharp.Common.Protocol
{
    public class GameMsg_ClKill : BaseGameMessage
    {
        public override GameMessage Type => GameMessage.ClientKill;

        public override bool PackError(MsgPacker packer)
        {
            return packer.Error;
        }

        public override bool UnPackError(UnPacker unpacker, ref string failedOn)
        {
            return unpacker.Error;
        }
    }
}