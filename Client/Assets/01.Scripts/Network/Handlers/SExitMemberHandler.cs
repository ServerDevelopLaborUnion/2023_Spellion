using System.Collections;
using System.Collections.Generic;
using Google.Protobuf;
using Packet;
using UnityEngine;

public class SExitMemberHandler : IPacketHandler
{
    public void Process(IMessage packet)
    {
        S_Exit_Member exit = packet as S_Exit_Member;
        LobbyUI lobbyUI = GameObject.FindObjectOfType<LobbyUI>();
        if(lobbyUI != null)
        {
            lobbyUI.FixMember(exit.FixedMembers);
        }
    }
}
