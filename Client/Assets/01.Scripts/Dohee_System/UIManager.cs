using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager
{
    public static UIManager Instance;
    [SerializeField]TextMeshProUGUI timeText = null;
    [SerializeField]TextMeshProUGUI redTeamScoreText = null;
    [SerializeField]TextMeshProUGUI blueTeamScoreText = null;
    [SerializeField]TextMeshProUGUI redModeScoreText = null;
    [SerializeField]TextMeshProUGUI blueModeScoreText = null;
    private int redTeamScore = 0;
    private int blueTeamScore = 0;
    public int RedTeamScore => redTeamScore;
    public int BlueTeamScore => blueTeamScore;
    private int redModeScore = 0;
    private int blueModeScore = 0;
    public int RedModeScore => redModeScore;
    public int BlueModeScore => blueModeScore;
    public void Init(){
        Dohee_GameManager.Instance.startGame += InGameLoad;
    }
    private void InGameLoad(){
        timeText = GameObject.Find("Timer/TimerText").GetComponent<TextMeshProUGUI>();
        redTeamScoreText = GameObject.Find("BlueTeamScore/BlueTeamScoreText").GetComponent<TextMeshProUGUI>();
        blueTeamScoreText = GameObject.Find("RedTeamScore/RedTeamScoreText").GetComponent<TextMeshProUGUI>();
        redModeScoreText = GameObject.Find("ModeScoreUI/RedModeScore/RedModeScoreText").GetComponent<TextMeshProUGUI>();
        blueModeScoreText = GameObject.Find("ModeScoreUI/BlueModeScore/BlueModeScoreText").GetComponent<TextMeshProUGUI>();
    }
    public void SetTimeText(float time){
        timeText.SetText(((int)time).ToString());
    }
    public void SetRoundScoreText(bool blueWin){
        if(blueWin) blueTeamScore++;
        else redTeamScore++;
        blueTeamScoreText.text = blueTeamScore.ToString();
        redTeamScoreText.text = redTeamScore.ToString();
        if(Dohee_GameManager.CurrentMode == GameMode.KillAll){
            blueModeScore = 5;
            redModeScore = 5;
        }
        if(Dohee_GameManager.CurrentMode == GameMode.TakePlace){
            blueModeScore = 0;
            redModeScore = 0;
        }
        blueModeScoreText.text = blueModeScore.ToString();
        redModeScoreText.text = redModeScore.ToString();
    }
    public void SetModeScoreText(){
        if(Dohee_GameManager.CurrentMode == GameMode.KillAll){
            blueModeScore = Dohee_GameManager.Instance.BluePlayer;
            redModeScore = Dohee_GameManager.Instance.RedPlayer;
        }
        blueModeScoreText.text = blueModeScore.ToString();
        redModeScoreText.text = redModeScore.ToString();
    }
}