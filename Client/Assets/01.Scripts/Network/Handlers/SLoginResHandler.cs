using System.Collections;
using System.Collections.Generic;
using Google.Protobuf;
using Packet;
using UnityEngine;

public class SLoginResHandler : IPacketHandler
{
    public void Process(IMessage packet)
    {
        S_Login_Res res = packet as S_Login_Res;
        
        if(res.Success)
        {
            GameManager.Instance.SetPlayerData(res.UserData);
        }
    }
}
