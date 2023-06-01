using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    public static SceneManagerScript Instance;
    private void Awake() {
        if(Instance != null) Destroy(gameObject);
        Instance = this;
    }
    public void GoLogInScene(){
        SceneManager.LoadScene(0);
    }
    public void GoLobbyScene(){
        SceneManager.LoadScene(1);
    }
    public void GoGameScene(){
        AsyncOperation async = SceneManager.LoadSceneAsync(2);
        StartCoroutine(Wait(async, Dohee_GameManager.Instance.InGameSetting, null, null));
    }
    public void ChangeGameMode(int mode){
        Dohee_GameManager.Instance.ChangeGameMode(mode);
    }
    private IEnumerator Wait(AsyncOperation async, Action action1, Action action2, Action action3){
        yield return new WaitUntil(() => async.isDone);
        action1?.Invoke();
        action2?.Invoke();
        action3?.Invoke();
    }
}
