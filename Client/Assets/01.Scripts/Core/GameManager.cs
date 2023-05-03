using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [SerializeField] private PoolingListSO _initPoolList;
    
    // Network
    private SocketManager _socketManager;
    [SerializeField] private string _connectionUrl;

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
}
