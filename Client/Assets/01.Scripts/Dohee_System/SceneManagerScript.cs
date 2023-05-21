using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript
{
    public static SceneManagerScript Instance;
    public void GoLogInScene(){
        SceneManager.LoadScene(0);
    }
    public void GoLobbyScene(){
        SceneManager.LoadScene(1);
    }
    public void GoGameScene(){
        SceneManager.LoadScene(2);
    }
    public void GameEnd(){
        GoLobbyScene();
    }
}
