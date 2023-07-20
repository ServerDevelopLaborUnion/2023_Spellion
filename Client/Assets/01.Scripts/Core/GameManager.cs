using System.Collections;
using System.Collections.Generic;
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

        // Create Socket Manager
        gameObject.AddComponent<SocketManager>();
        SocketManager.Instance.Init($"{_ip}:{_port}");
        SocketManager.Instance.Connection();
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
    }
}