using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager
{
    public static UIManager Instance;
    [SerializeField]TextMeshProUGUI timeText = null;
    [SerializeField]TextMeshProUGUI redTeamScoreText = null;
    [SerializeField]TextMeshProUGUI blueTeamScoreText = null;
    [SerializeField]TextMeshProUGUI redPlaceScoreText = null;
    [SerializeField]TextMeshProUGUI bluePlaceScoreText = null;
    private int redTeamScore = 0;
    private int blueTeamScore = 0;
    public int RedTeamScore => redTeamScore;
    public int BlueTeamScore => blueTeamScore;
    private int redPlaceScore = 0;
    private int bluePlaceScore = 0;
    public int RedPlaceScore => redPlaceScore;
    public int BluePlaceScore => bluePlaceScore;
    public void Init() {
        timeText = GameObject.Find("TimerText").GetComponent<TextMeshProUGUI>();
        redTeamScoreText = GameObject.Find("BlueTeamScoreText").GetComponent<TextMeshProUGUI>();
        blueTeamScoreText = GameObject.Find("RedTeamScoreText").GetComponent<TextMeshProUGUI>();
        redPlaceScoreText = GameObject.Find("TakePlaceModeUI/RedPlaceScoreText").GetComponent<TextMeshProUGUI>();
        bluePlaceScoreText = GameObject.Find("TakePlaceModeUI/BluePlaceScoreText").GetComponent<TextMeshProUGUI>();
    }
    public void SetTimeText(float time){
        timeText.SetText(((int)time).ToString());
    }
    public void SetScoreText(bool blueWin){
        if(blueWin) blueTeamScore++;
        else redTeamScore++;
        blueTeamScoreText.text = blueTeamScore.ToString();
        redTeamScoreText.text = redTeamScore.ToString();
    }
    public void SetPlaceScore(bool blueGetPoint){
        if(blueGetPoint) bluePlaceScore++;
        else redPlaceScore++;
        bluePlaceScoreText.text = bluePlaceScore.ToString();
        redPlaceScoreText.text = redPlaceScore.ToString();
    }
}