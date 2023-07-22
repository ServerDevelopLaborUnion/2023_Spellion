using System.Collections;
using System.Collections.Generic;
using Google.Protobuf;
using Packet;
using UnityEngine;

public class SRegisterResHandler : IPacketHandler
{
    public void Process(IMessage packet)
    {
        S_Register_Res res = packet as S_Register_Res;

        if(res.Success)
        {
            PlayerPrefs.SetString("name", res.UserData.Name);
            GameManager.Instance.LoadScene("MainLobby");
            GameManager.Instance.SetPlayerData(res.UserData);
        }
    }
}
