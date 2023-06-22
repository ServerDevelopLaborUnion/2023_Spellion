using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [SerializeField] private PoolingListSO _initPoolList;

    private Transform _player = null;
    public Transform Player => _player;
    
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
        if(_initPoolList == null) 
        {
            Debug.LogWarning("Init Pool list is null. PoolManager will be null");
            return;
        }
        PoolManager.Instance = new PoolManager(transform);
        _initPoolList.PoolList.ForEach(p =>
        {
            PoolManager.Instance.CreatePool(p.Prefab, p.Count);
        });
    }

    public void FindPlayer()
    {
        _player = GameObject.Find("Player").transform;
    }
}
