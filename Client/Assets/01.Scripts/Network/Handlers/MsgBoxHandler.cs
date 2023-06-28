using System.Collections;
using System.Collections.Generic;
using Google.Protobuf;
using Packet;
using UnityEngine;

public class MsgBoxHandler : IPacketHandler
{
    public void Process(IMessage packet)
    {
        MsgBox msgBox = (packet as MsgBox);
        Debug.Log(msgBox.Context);
        float rtt = Time.time - msgBox.Time;
        Debug.Log($"RTT: {rtt * 1000f}ms");
        Debug.Log($"1/2 RTT: {rtt * 500f}ms");
    }
}
