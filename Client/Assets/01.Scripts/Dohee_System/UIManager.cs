using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager
{
    public static UIManager Instance;
    TextMeshProUGUI timeText = null;
    TextMeshProUGUI redTeamScoreText = null;
    TextMeshProUGUI blueTeamScoreText = null;
    TextMeshProUGUI redModeScoreText = null;
    TextMeshProUGUI blueModeScoreText = null;
    public void Init(){
        
    }
    public void InGameLoad(GameMode currentMode){
        timeText = GameObject.Find("TimerText").GetComponent<TextMeshProUGUI>();
        redTeamScoreText = GameObject.Find("BlueTeamScoreText").GetComponent<TextMeshProUGUI>();
        blueTeamScoreText = GameObject.Find("RedTeamScoreText").GetComponent<TextMeshProUGUI>();
        redModeScoreText = GameObject.Find("RedModeScoreText").GetComponent<TextMeshProUGUI>();
        blueModeScoreText = GameObject.Find("BlueModeScoreText").GetComponent<TextMeshProUGUI>();

        SetModeUI(currentMode);
    }
    private void SetModeUI(GameMode currentMode){
        // 모드에 맞는 UI를 로드
        if(currentMode == GameMode.KillAll){
            // 팀 데스메치 UI 로드
        }
    }
    public void SetTimeText(float time){
        timeText.SetText(((int)time).ToString());
    }
    public void SetRoundScoreText(int blue, int red){
        blueTeamScoreText.text = blue.ToString();
        redTeamScoreText.text = red.ToString();
    }
    public void SetModeScoreText(int blue, int red){
        blueModeScoreText.text = blue.ToString();
        redModeScoreText.text = red.ToString();
    }
}