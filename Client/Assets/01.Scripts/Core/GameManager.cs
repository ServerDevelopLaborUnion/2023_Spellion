using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [SerializeField] private PoolingListSO _initPoolList;

    [SerializeField] private GameObject _playerPref;
    private GameObject _player = null;
    public GameObject Player => _player;
    
    // Network
    [SerializeField] private string _connectionUrl;
    private SocketManager _socketManager;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple GameManager is running");
        }

        Instance = this;
        
        CreateSocketManager();
        CreatePool();
    }

    private void CreateSocketManager()
    {
        _socketManager = gameObject.AddComponent<SocketManager>();
        SocketManager.Instance.Init(_connectionUrl);
        SocketManager.Instance.Connection();
    }

    private void CreatePool()
    {
        PoolManager.Instance = new PoolManager(transform);
        _initPoolList.PoolList.ForEach(p =>
        {
            PoolManager.Instance.CreatePool(p.Prefab, p.Count);
        });
    }

    public void CreatePlayer()
    {
        _player = Instantiate(_playerPref, new Vector3(0, 1, 0), Quaternion.identity);
        // _player.transform.position = new Vector3(0, 1, 0);
    }
}
