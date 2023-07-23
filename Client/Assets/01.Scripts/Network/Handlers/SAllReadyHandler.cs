using System.Collections;
using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;

public class SAllReadyHandler : IPacketHandler
{
    public void Process(IMessage packet)
    {
        LobbyUI lobbyUI = GameObject.FindObjectOfType<LobbyUI>();
        if(lobbyUI != null)
        {
            //
        }
    }
}
