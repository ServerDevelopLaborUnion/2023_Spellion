using System;
using System.Collections;
using System.Collections.Generic;
using Packet;
using UnityEngine;
using UnityEngine.SceneManagement;
using Kinemation.FPSFramework.Runtime.FPSAnimator;

public class GameManager : MonoBehaviour
{
    // Singleton
    private static GameManager _instance = null;
    public static GameManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
                if(_instance == null)
                {
                    GameObject manager = new GameObject("Manager");
                    _instance = manager.AddComponent<GameManager>();
                    DontDestroyOnLoad(manager);
                }
            }
            return _instance;
        }
    }

    private Transform _player;
    public Transform Player 
    {
        get
        {
            if(_player == null)
            {
                _player = FindObjectOfType<FPSAnimController>().transform;
                if(_player == null)
                {
                    Debug.LogError("Player Cannot Found");
                    throw null;
                }
            }
            return _player;
        }
    }

    private Camera _playerCam;
    public Camera PlayerCam
    {
        get
        {
            if(_playerCam == null)
            {
                _playerCam = Player.GetComponentInChildren<Camera>();
                if(_playerCam == null)
                {
                    Debug.LogError("PlayerCam Cannot Found");
                    throw null;
                }
            }
            return _playerCam;
        }
    }

    [SerializeField] 
    private string _ip, _port;

    [SerializeField]
    private float _minLoadTime = 3f;

    public User MyData { get => _myData; }
    private User _myData;

    private ManagerUI _managerUI;

    private Dictionary<string, Action<AsyncOperation>> _loadSceneCallback = new Dictionary<string, Action<AsyncOperation>>();

    private void Awake()
    {
        // Singleton
        if(_instance == null)
        {
            _instance = this;
            gameObject.name = "Manager";
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogError("Multiple GameManager is running, Destroy This!");
            Destroy(gameObject);
            return;
        }

        _managerUI = GetComponentInChildren<ManagerUI>();

        // Create Socket Manager
        gameObject.AddComponent<SocketManager>();
        string url = $"ws://{_ip}:{_port}";
        Debug.Log($"Start to connect {url}");
        SocketManager.Instance.Init(url);
        SocketManager.Instance.Connection();

        _loadSceneCallback["MainLobby"] = (oper) => {};
        _loadSceneCallback["Game"] = (oper) => {};

#if UNITY_EDITOR
        SocketManager.Instance.RegisterSend(MSGID.CLoginReq, new C_Login_Req{Name = "곽석현"});
        LoadScene("MainLobby");
#else
        if(PlayerPrefs.GetString("name") == string.Empty)
        {
            _managerUI.GetName();
        }
        else
        {
            SocketManager.Instance.RegisterSend(MSGID.CLoginReq, new C_Login_Req{Name = PlayerPrefs.GetString("name")});
            LoadScene("MainLobby");
        }
#endif
    }

    public void LoadScene(string sceneName, Action callback = null)
    {
        StartCoroutine(LoadSceneCoroutine(sceneName, callback));
    }

    public void LoadSceneImmediate(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator LoadSceneCoroutine(string sceneName, Action callback = null)
    {
        AsyncOperation loadScene = SceneManager.LoadSceneAsync(sceneName);
        _managerUI.ShowLoadingBar();
        float timer = _minLoadTime;
        while(loadScene.isDone && timer <= 0f)
        {
            timer -= Time.deltaTime;
            _managerUI.SetLoadingProgress(loadScene.progress * 100f);
            yield return null;
        }
        loadScene.completed += (oper) => {
            _managerUI.HideLoadingBar();
            callback?.Invoke();
            _loadSceneCallback[sceneName].Invoke(oper);
        };
        _loadSceneCallback[sceneName] = (oper) => {};
    }

    public void SetPlayerData(User userData)
    {
        _myData = userData;
        LobbyUI lobbyUI = FindObjectOfType<LobbyUI>();
        if(lobbyUI != null)
        {
            lobbyUI.SetUserData(MyData);
        }
        else
        {
            _loadSceneCallback["MainLobby"] += (oper) => {};
        }
    }

    public PlayerManager CreatePlayerManager()
    {
        return gameObject.AddComponent<PlayerManager>();
    }
}