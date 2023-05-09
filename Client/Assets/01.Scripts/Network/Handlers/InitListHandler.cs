using System.Collections;
using System.Collections.Generic;
using Google.Protobuf;
using Packet;
using UnityEngine;

public class InitListHandler : IPacketHandler
{
    public void Process(IMessage packet)
    {
        // Player Create
        GameManager.Instance.CreatePlayer();
        // Remote Manager Create
        RemoteManager manager = GameManager.Instance.CreateRemoteManager();
        manager.Init(packet as PlayerInfoList);
    }
}
