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

    private bool _isPause = true;
    public bool IsPause => _isPause;

    public UnityEvent OnPause;
    public UnityEvent OnResume;

    private GameUI _gameUI;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple GameManager is running");
        }

        Instance = this;

        _gameUI = FindObjectOfType<GameUI>();
        
        SetPause(true);
        CreateSocketManager();
        CreatePool();
    }

    private void CreateSocketManager()
    {
        _socketManager = gameObject.AddComponent<SocketManager>();
        SocketManager.Instance.Init(_connectionUrl);

        SocketManager.Instance.OnConnect += () => SetPause(false);
        SocketManager.Instance.OnDisconnect += () => SetPause(true);

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

    public void SetPause(bool value)
    {
        _gameUI.SetPause(value);
        Cursor.lockState = value ? CursorLockMode.None : CursorLockMode.Locked;
        if(value) OnPause?.Invoke();
        else OnResume?.Invoke();
    }
}
