using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Packet;
using Google.Protobuf;

public class SJoinResHandler : IPacketHandler
{
    public void Process(IMessage packet)
    {
        S_Join_Res res = packet as S_Join_Res;
        if(res.Success)
        {
            LobbyUI lobbyUI = GameObject.FindObjectOfType<LobbyUI>();
            if(lobbyUI != null)
            {
                if(res.IsOwner)
                {
                    lobbyUI.InitRoom();
                }
                else
                {
                    lobbyUI.InitRoom(res.Index, res.Members);
                }
            }
        }
    }
}
