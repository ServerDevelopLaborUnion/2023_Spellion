using System.Collections;
using System.Collections.Generic;
using Google.Protobuf;
using Packet;
using UnityEngine;

public class PlayerInfoListHandler : IPacketHandler
{
    public void Process(IMessage packet)
    {
        PlayerInfoList infoList = packet as PlayerInfoList;
        for(int i = 0; i < infoList.List.Count; i++)
        {
            RemoteManager.Instance.SetRemote(infoList.List[i]);
        }
    }
}
