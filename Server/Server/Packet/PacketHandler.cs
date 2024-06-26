﻿using Server.Session;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server {
    class PacketHandler 
    {
        // 돌 놓는거
        public static void C_MoveStoneHandler(PacketSession session, IPacket packet)
        {
            C_SelectHole movePacket = packet as C_SelectHole;
            ClientSession clientSession = session as ClientSession;
            if (clientSession.Room == null)
                return;
            Console.WriteLine($"구멍 숫자 : {movePacket.selectHole}");
            Console.WriteLine($"Destiation ID : {movePacket.destinationId}");
            
            GameRoom room = clientSession.Room;
            room.Move(clientSession, movePacket);

            //GameRoom room = clientSession.Room;
            //room.Leave(clientSession);
        }

        // 클라가 떠났을 때, room에서 내쫓는 동작
        public static void C_LeaveGameHandler(PacketSession session, IPacket packet)
        {
            ClientSession clientSession = session as ClientSession;

            if (clientSession.Room == null)
                return;

            GameRoom room = clientSession.Room;
            room.Leave(clientSession);
        }
    }
}
