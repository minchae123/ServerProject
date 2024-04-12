﻿using DummyClient.Session;
using ServerCore;
using UnityEngine;

namespace DummyClient
{
    class PacketHandler {
        // 어떤 세션에서 조립된 패킷인지, 어떤 내용의 패킷인지

        // 플레이어 입장 신호 보내기
        public static void S_BroadcastEnterGameHandler(PacketSession session, IPacket packet)
        {
            S_BroadcastEnterGame pkt = packet as S_BroadcastEnterGame;
            ServerSession serverSession = session as ServerSession;

            PlayerManager.Instance.EnterGame(pkt);
        }

        // 플레이어 퇴장 신호 보내기
        public static void S_BroadcastLeaveGameHandler(PacketSession session, IPacket packet)
        {
            S_BroadcastLeaveGame pkt = packet as S_BroadcastLeaveGame;
            ServerSession serverSession = session as ServerSession;

            PlayerManager.Instance.LeaveGame(pkt);
        }

        // 현재 플레이어 리스트
        public static void S_PlayerListHandler(PacketSession session, IPacket packet)
        {
            S_PlayerList pkt = packet as S_PlayerList;
            ServerSession serverSession = session as ServerSession;

            PlayerManager.Instance.Add(pkt);
        }
       


        public static void S_BroadCastStoneHandler(PacketSession session, IPacket packet)
        {
            S_BroadCastStone pkt = packet as S_BroadCastStone;
            ServerSession serverSession = session as ServerSession;

            PlayerManager.Instance.BroadCastStone(pkt);
        }

    }
}


