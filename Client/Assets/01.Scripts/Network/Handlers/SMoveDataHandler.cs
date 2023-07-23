using System.Collections;
using System.Collections.Generic;
using Google.Protobuf;
using Packet;
using UnityEngine;

public class SMoveDataHandler : IPacketHandler
{
    public void Process(IMessage packet)
    {
        S_Move_Data moveData = packet as S_Move_Data;
        PlayerManager.Instance.UpdateRemote(moveData.Pos.ToUnityVector(), moveData.EulerAngle.ToUnityRotation(), moveData.Id, moveData.Team);
    }
}
