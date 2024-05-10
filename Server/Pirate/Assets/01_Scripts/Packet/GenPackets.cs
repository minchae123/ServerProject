using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DummyClient
{
    public enum PacketID
    {
        S_BroadcastEnterGame = 1,
        C_LeaveGame = 2,
        S_BroadcastLeaveGame = 3,
        S_PlayerList = 4,

        C_SelectHole = 5,
        S_BroadCastHole = 6,
    }

    public interface IPacket
    {
        ushort Protocol { get; }
        void Read(ArraySegment<byte> segment);
        ArraySegment<byte> Write();
    }

    public class C_SelectHole : IPacket
    {
        public int holeNumber;
        public int destinationId;
        public ushort Protocol { get { return (ushort)PacketID.C_SelectHole; } }

        public void Read(ArraySegment<byte> segment)
        {
            ushort count = 0;
            count += sizeof(ushort);
            count += sizeof(ushort);
            this.holeNumber = BitConverter.ToInt32(segment.Array, segment.Offset + count);
            count += sizeof(int);
            this.destinationId = BitConverter.ToInt32(segment.Array,segment.Offset + count);
            count += sizeof(int);
        }

        public ArraySegment<byte> Write()
        {
            ArraySegment<byte> segment = SendBufferHelper.Open(4096);
            ushort count = 0;

            count += sizeof(ushort);
            Array.Copy(BitConverter.GetBytes((ushort)PacketID.C_SelectHole), 0, segment.Array, segment.Offset + count, sizeof(ushort));
            count += sizeof(ushort);
            Array.Copy(BitConverter.GetBytes(this.holeNumber), 0, segment.Array, segment.Offset + count, sizeof(int));
            count += sizeof(int);
            Array.Copy(BitConverter.GetBytes(this.destinationId), 0, segment.Array, segment.Offset + count, sizeof(int));
            count += sizeof(int);

            Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

            return SendBufferHelper.Close(count);
        }
    }

    public class S_BroadCastHole : IPacket
    {
        public int holeNumber;
        public ushort Protocol { get { return (ushort)PacketID.S_BroadCastHole; } }

        public void Read(ArraySegment<byte> segment)
        {
            ushort count = 0;
            count += sizeof(ushort);
            count += sizeof(ushort);
            this.holeNumber = BitConverter.ToInt32(segment.Array, segment.Offset + count);
            count += sizeof(int);
        }

        public ArraySegment<byte> Write()
        {
            ArraySegment<byte> segment = SendBufferHelper.Open(4096);
            ushort count = 0;

            count += sizeof(ushort);
            Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_BroadCastHole), 0, segment.Array, segment.Offset + count, sizeof(ushort));
            count += sizeof(ushort);
            Array.Copy(BitConverter.GetBytes(this.holeNumber), 0, segment.Array, segment.Offset + count, sizeof(int));
            count += sizeof(int);

            Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

            return SendBufferHelper.Close(count);
        }
    }


    public class S_BroadcastEnterGame : IPacket
    {
        public int playerId;

        public ushort Protocol { get { return (ushort)PacketID.S_BroadcastEnterGame; } }

        public void Read(ArraySegment<byte> segment)
        {
            ushort count = 0;
            count += sizeof(ushort);
            count += sizeof(ushort);
            this.playerId = BitConverter.ToInt32(segment.Array, segment.Offset + count);
            count += sizeof(int);
        }

        public ArraySegment<byte> Write()
        {
            ArraySegment<byte> segment = SendBufferHelper.Open(4096);
            ushort count = 0;

            count += sizeof(ushort);
            Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_BroadcastEnterGame), 0, segment.Array, segment.Offset + count, sizeof(ushort));
            count += sizeof(ushort);
            Array.Copy(BitConverter.GetBytes(this.playerId), 0, segment.Array, segment.Offset + count, sizeof(int));
            count += sizeof(int);

            Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

            return SendBufferHelper.Close(count);
        }
    }

    public class C_LeaveGame : IPacket
    {
        public ushort Protocol { get { return (ushort)PacketID.C_LeaveGame; } }

        public void Read(ArraySegment<byte> segment)
        {
            ushort count = 0;
            count += sizeof(ushort);
            count += sizeof(ushort);

        }

        public ArraySegment<byte> Write()
        {
            ArraySegment<byte> segment = SendBufferHelper.Open(4096);
            ushort count = 0;

            count += sizeof(ushort);
            Array.Copy(BitConverter.GetBytes((ushort)PacketID.C_LeaveGame), 0, segment.Array, segment.Offset + count, sizeof(ushort));
            count += sizeof(ushort);


            Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

            return SendBufferHelper.Close(count);
        }
    }

    public class S_BroadcastLeaveGame : IPacket
    {
        public int playerId;

        public ushort Protocol { get { return (ushort)PacketID.S_BroadcastLeaveGame; } }

        public void Read(ArraySegment<byte> segment)
        {
            ushort count = 0;
            count += sizeof(ushort);
            count += sizeof(ushort);
            this.playerId = BitConverter.ToInt32(segment.Array, segment.Offset + count);
            count += sizeof(int);
        }

        public ArraySegment<byte> Write()
        {
            ArraySegment<byte> segment = SendBufferHelper.Open(4096);
            ushort count = 0;

            count += sizeof(ushort);
            Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_BroadcastLeaveGame), 0, segment.Array, segment.Offset + count, sizeof(ushort));
            count += sizeof(ushort);
            Array.Copy(BitConverter.GetBytes(this.playerId), 0, segment.Array, segment.Offset + count, sizeof(int));
            count += sizeof(int);

            Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

            return SendBufferHelper.Close(count);
        }
    }

    public class S_PlayerList : IPacket
    {
        public class Player
        {
            public bool isSelf;
            public int playerId;

            public void Read(ArraySegment<byte> segment, ref ushort count)
            {
                this.isSelf = BitConverter.ToBoolean(segment.Array, segment.Offset + count);
                count += sizeof(bool);
                this.playerId = BitConverter.ToInt32(segment.Array, segment.Offset + count);
                count += sizeof(int);
            }

            public bool Write(ArraySegment<byte> segment, ref ushort count)
            {
                bool success = true;
                Array.Copy(BitConverter.GetBytes(this.isSelf), 0, segment.Array, segment.Offset + count, sizeof(bool));
                count += sizeof(bool);
                Array.Copy(BitConverter.GetBytes(this.playerId), 0, segment.Array, segment.Offset + count, sizeof(int));
                count += sizeof(int);

                return success;
            }
        }
        public List<Player> players = new List<Player>();

        public ushort Protocol { get { return (ushort)PacketID.S_PlayerList; } }

        public void Read(ArraySegment<byte> segment)
        {
            ushort count = 0;
            count += sizeof(ushort);
            count += sizeof(ushort);
            this.players.Clear();
            ushort playerLen = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
            count += sizeof(ushort);
            for (int i = 0; i < playerLen; i++)
            {
                Player player = new Player();
                player.Read(segment, ref count);
                players.Add(player);
            }
        }

        public ArraySegment<byte> Write()
        {
            ArraySegment<byte> segment = SendBufferHelper.Open(4096);
            ushort count = 0;

            count += sizeof(ushort);
            Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_PlayerList), 0, segment.Array, segment.Offset + count, sizeof(ushort));
            count += sizeof(ushort);
            Array.Copy(BitConverter.GetBytes((ushort)this.players.Count), 0, segment.Array, segment.Offset + count, sizeof(ushort));
            count += sizeof(ushort);
            foreach (Player player in this.players)
                player.Write(segment, ref count);

            Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

            return SendBufferHelper.Close(count);
        }
    }
}