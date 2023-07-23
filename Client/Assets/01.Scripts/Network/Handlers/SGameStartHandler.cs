using System.Collections;
using System.Collections.Generic;
using Google.Protobuf;
using Kinemation.FPSFramework.Runtime.FPSAnimator;
using Kinemation.FPSFramework.Runtime.Core.Components;
using Packet;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class SGameStartHandler : IPacketHandler
{
    public void Process(IMessage packet)
    {
        S_Game_Start start = packet as S_Game_Start;
        GameManager.Instance.LoadScene("Game", () => {
            Vector3 spawnPos = new Vector3(
                start.SpawnPos.X,
                start.SpawnPos.Y,
                start.SpawnPos.Z
            );
            GameManager.Instance.Player.transform.position = spawnPos;
            GameManager.Instance.Player.GetComponent<FPSAnimController>().enabled = false;
            GameManager.Instance.Player.GetComponentInChildren<CoreAnimComponent>().enabled = false;
            GameManager.Instance.PlayerCam.gameObject.SetActive(false);
            
            Debug.Log("set pos");
            Debug.Log(spawnPos);
        });
    }
}
