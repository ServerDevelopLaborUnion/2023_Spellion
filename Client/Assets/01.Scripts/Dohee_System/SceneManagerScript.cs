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
        SceneManager.LoadScene(2);
    }
    public void GameEnd(){
        GoLobbyScene();
    }
    public void ChangeGameMode(int mode){
        Dohee_GameManager.ChangeGameMode(mode);
    }
}
