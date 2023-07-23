using System.Collections;
using System.Collections.Generic;
using Kinemation.FPSFramework.Runtime.FPSAnimator;
using Packet;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class PlayerManager : MonoBehaviour
{
    private static PlayerManager _instance;
    public static PlayerManager Instance
    {
        get 
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<PlayerManager>();
                if(_instance == null)
                {
                    _instance = GameManager.Instance.CreatePlayerManager();
                }
            }
            return _instance;
        }
    }

    private RemotePlayer _playerPrefab;

    private Transform _blue, _red;

    private Dictionary<int, RemotePlayer> _remotes = new Dictionary<int, RemotePlayer>();

    private void Awake()
    {
        _blue = new GameObject("BlueTeam").transform;
        _red = new GameObject("RedTeam").transform;

        _blue.position = _red.position = Vector3.zero;

        transform.position = Vector3.zero;
        if(_instance == null)
        {
            _instance = this;
        }
        else
        {
            Debug.LogError("Multiple PlayerManager is running, Destroy This!");
            return;
        }
    }

    public void CreateRemote(Vector3 spawnPos, Team team , int name)
    {
        RemotePlayer player;

        if(team == Team.Blue)
            player = Instantiate(_playerPrefab, spawnPos, Quaternion.identity);
        else
            player = Instantiate(_playerPrefab, spawnPos, Quaternion.identity);
        
        _remotes.Add(name, player);
    }

    public void UpdateRemote(Vector3 pos, Quaternion rot, int name)
    {
        if(_remotes.TryGetValue(name, out RemotePlayer remote))
        {
            remote.SetPosAndRot(pos, rot);
        }
    }
}
