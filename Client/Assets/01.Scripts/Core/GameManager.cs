using System;
using System.Collections;
using System.Collections.Generic;
using Packet;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        if(PlayerPrefs.GetString("name") == string.Empty)
        {
            _managerUI.GetName();
        }
        else
        {
            SocketManager.Instance.RegisterSend(MSGID.CLoginReq, new C_Login_Req{Name = "kwak1s1h"});
            LoadScene("MainLobby");
        }
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneCoroutine(sceneName));
    }

    public void LoadSceneImmediate(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        AsyncOperation loadScene = SceneManager.LoadSceneAsync(sceneName);
        float timer = _minLoadTime;
        while(loadScene.isDone && timer <= 0f)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        loadScene.completed += _loadSceneCallback[sceneName];
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
}