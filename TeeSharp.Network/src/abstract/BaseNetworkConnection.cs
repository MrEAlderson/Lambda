﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using TeeSharp.Common.Config;
using TeeSharp.Core;
using TeeSharp.Network.Enums;

namespace TeeSharp.Network
{
    public abstract class BaseNetworkConnection : BaseInterface
    {
        public virtual ConnectionState State { get; protected set; }
        public virtual IPEndPoint EndPoint { get; protected set; }
        public virtual string Error { get; protected set; }

        public virtual long ConnectedAt { get; protected set; }

        protected virtual int Sequence { get; set; }
        protected virtual int Ack { get; set; }
        protected virtual int PeerAck { get; set; }

        protected virtual long LastReceiveTime { get; set; }
        protected virtual long LastSendTime { get; set; }

        protected virtual bool RemoteClosed { get; set; }
        protected virtual NetworkChunkConstruct ResendChunkConstruct { get; set; }
        protected virtual IList<NetworkChunkResend> ChunksFoResends { get; set; }

        protected virtual uint Token { get; set; }
        protected virtual uint PeerToken { get; set; }
        protected virtual UdpClient UdpClient { get; set; } 
        protected virtual BaseConfig Config { get; set; }

        public abstract void Init(UdpClient udpClient);
        public abstract bool Connect(IPEndPoint endPoint);
        public abstract void Disconnect(string reason);
        public abstract void SetToken(uint token);

        public abstract void Update();
        public abstract int Flush();
        public abstract bool Feed(NetworkChunkConstruct packet, IPEndPoint remote);
        public abstract bool QueueChunk(ChunkFlags flags, byte[] data, int dataSize);
        public abstract void SendPacketConnless(byte[] data, int dataSize);

        protected abstract void Reset();
        protected abstract void AckChunks(int ack);
        protected abstract bool QueueChunkEx(ChunkFlags flags, 
            byte[] data, int dataSize, int sequence);
        protected abstract void SendConnectionMsg(ConnectionMessages msg, 
            string extra);
        protected abstract void SendConnectionMsg(ConnectionMessages msg, 
            byte[] extra, int extraSize);
        protected abstract void SendConnectionMsgWithToken(ConnectionMessages msg);

        protected abstract void ResendChunk(NetworkChunkResend resend);
        protected abstract void Resend();
    }
}