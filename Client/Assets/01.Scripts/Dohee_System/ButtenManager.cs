using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtenManager : MonoBehaviour
{
    private Button[] button;
    private void Start() {
        button = GetComponentsInChildren<Button>();
        for(int i = 0; i < button.Length; i++){
            if(button[i].name == "LoginButtom") button[i].onClick.AddListener(SceneManagerScript.Instance.GoLobbyScene);
            if(button[i].name == "GameStart") button[i].onClick.AddListener(SceneManagerScript.Instance.GoGameScene);
        }
    }
}
