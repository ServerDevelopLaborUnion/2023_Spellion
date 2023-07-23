using System;
using System.Collections;
using System.Collections.Generic;
using Kinemation.FPSFramework.Runtime.FPSAnimator;
using Packet;
using UnityEngine;
using UnityEngine.SceneManagement;
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
                    if(SceneManager.GetActiveScene().name == "Game")
                        _instance = GameManager.Instance.CreatePlayerManager();
                }
            }
            return _instance;
        }
    }

    private GameObject _playerPrefab;

    private Transform _blue, _red;

    private Dictionary<int, RemotePlayer> _remotes = new Dictionary<int, RemotePlayer>();

    public void Init()
    {
        if(SceneManager.GetActiveScene().name == "Game")
        {
            _blue = new GameObject("BlueTeam").transform;
            _red = new GameObject("RedTeam").transform;
            Debug.Log(_blue.gameObject);

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
    }

    public void CreateRemote(Vector3 spawnPos, Team team , int name)
    {
        GameObject remote;

        if(team == Team.Blue)
            remote = Instantiate(_playerPrefab, spawnPos, Quaternion.identity, _blue);
        else
            remote = Instantiate(_playerPrefab, spawnPos, Quaternion.identity, _red);
        
        _remotes.Add(name, remote.GetComponent<RemotePlayer>());
    }

    public void UpdateRemote(Vector3 pos, Quaternion rot, int name, Team team)
    {
        if(_remotes.TryGetValue(name, out RemotePlayer remote))
        {
            remote.SetPosAndRot(pos, rot);
        }
        else 
        {
            CreateRemote(pos, team, name);
        }
    }

    public void SetPrefab(GameObject playerPref)
    {
        _playerPrefab = Resources.Load<GameObject>("RemoteCharacter");
    }
}
