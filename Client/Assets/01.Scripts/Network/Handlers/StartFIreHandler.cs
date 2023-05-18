using System.Collections;
using System.Collections.Generic;
using Google.Protobuf;
using Packet;
using UnityEngine;

public class StartFIreHandler : IPacketHandler
{
    public void Process(IMessage packet)
    {
        RemoteManager.Instance.SetStartFire(packet as UUID);
    }
}
