using System.Collections;
using System.Collections.Generic;
using Google.Protobuf;
using Packet;
using UnityEngine;

public class SJoinedHandler : IPacketHandler
{
    public void Process(IMessage packet)
    {
        S_Joined joined = packet as S_Joined;

        LobbyUI lobbyUI = GameObject.FindObjectOfType<LobbyUI>();
        if(lobbyUI != null)
        {
            lobbyUI.AddMember(joined.Member);
        }
    }
}
