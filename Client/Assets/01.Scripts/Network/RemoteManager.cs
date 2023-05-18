using System;
using System.Collections;
using System.Collections.Generic;
using Packet;
using UnityEngine;

public class RemoteManager : MonoBehaviour
{
    private static RemoteManager _instance;
    public static RemoteManager Instance
    {
        get
        {
            if(_instance == null)
                _instance = FindObjectOfType<RemoteManager>();
            return _instance;
        }
    }

    private Dictionary<string, RemotePlayer> _remotePlayers = new Dictionary<string, RemotePlayer>();

    public void Init(PlayerInfoList list)
    {
        foreach(PlayerInfo pInfo in list.List)
        {
            RemotePlayer rPlayer = PoolManager.Instance.Pop("RemotePlayer").GetComponent<RemotePlayer>();
            _remotePlayers.Add(pInfo.Uuid, rPlayer);
            rPlayer.Init(pInfo);
        }
    }

    public void CreateRemotePlayer(PlayerInfo pInfo)
    {
        RemotePlayer player = PoolManager.Instance.Pop("RemotePlayer").GetComponent<RemotePlayer>();
        player.Init(pInfo);
        _remotePlayers.Add(pInfo.Uuid, player);
    }

    public void SetRemote(PlayerInfo playerInfo)
    {
        if(_remotePlayers.ContainsKey(playerInfo.Uuid))
        _remotePlayers[playerInfo.Uuid].SetPosAndRot(playerInfo.Pos, playerInfo.Rot);
    }

    public void SetStartFire(UUID uuid)
    {
        if(_remotePlayers.ContainsKey(uuid.Value))
        _remotePlayers[uuid.Value].StartFire();
    }

    public void SetStopFire(UUID uuid)
    {
        if(_remotePlayers.ContainsKey(uuid.Value))
        _remotePlayers[uuid.Value].StopFire();
    }
}
