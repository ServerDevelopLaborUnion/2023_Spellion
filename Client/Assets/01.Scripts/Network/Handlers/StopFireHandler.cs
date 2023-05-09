
using System.Collections;
using System.Collections.Generic;
using Google.Protobuf;
using Packet;
using UnityEngine;

public class StopFireHandler : IPacketHandler
{
    public void Process(IMessage packet)
    {
        RemoteManager.Instance.SetStopFire(packet as UUID);
    }
}
