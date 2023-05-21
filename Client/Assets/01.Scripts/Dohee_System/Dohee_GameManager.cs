using System;
using System.Collections;
using UnityEngine;

public enum GameMode{ // 게임모드 임의로 지정 점령은 스코어가 100이되면 이기고 섬멸은 다 죽여야 이김
    KillAll = 1,
    TakePlace = 2
}
public class Dohee_GameManager : MonoBehaviour
{
    public static Dohee_GameManager Instance = null;
    private Coroutine currentRound = null;
    private GameMode currentMode = GameMode.KillAll;
    public GameMode CurrentMode => currentMode;
    private bool game; // 전체 게임의 값
    public bool Game => game;
    private bool phase; // 한칸 한칸 넘기는 값
    public bool Phase => phase;
    private bool isInGame; // 승리 계산하는지 확인하는 값
    public bool IsInGame => isInGame;
    private int bluePlayer = 5;
    public int BluePlayer{
        get => bluePlayer;
        set => bluePlayer = Mathf.Clamp(value, 0, 5);
    }
    private int redPlayer = 5;
    public int RedPlayer{
        get => redPlayer;
        set => redPlayer = Mathf.Clamp(value, 0, 5);
    }
    public event Action startGame; // 게임 시작 (로딩)
    public event Action endGame; // 게임 끝 (결과 창)
    public event Action waitRound; // 픽하는 라운드
    public event Action startRound; // 라운드 시작
    public event Action endRound; // 라운드 끝
    private void Awake(){
        if(Instance != null) Destroy(gameObject);
        Instance = this;
        CreateManagers();
        Setting();
    }
    private void Start(){
        StartCoroutine(StartGame());
    }
    private void CreateManagers(){
        TimerManager.Instance = new TimerManager();
        SceneManagerScript.Instance = new SceneManagerScript();
        UIManager.Instance = new UIManager();
        UIManager.Instance.Init();
    }
    private void Setting(){
        game = true;
    }
    private IEnumerator StartGame(){
        startGame?.Invoke();
        yield return new WaitUntil(() => true);
        while(game){
            phase = true;
            waitRound?.Invoke();
            yield return new WaitUntil(() => !phase);
            phase = true;
            isInGame = true;
            startRound?.Invoke();
            yield return new WaitUntil(() => !phase);
            phase = true;
            endRound?.Invoke();
            yield return new WaitUntil(() => !phase);
            WinCheck();
        }
        endGame?.Invoke();
    }
    public void TimerStart(float time){ // 타이머 돌리는 함수
        if(currentRound != null)
            StopCoroutine(currentRound);
        currentRound = StartCoroutine(Timer(time));
    }
    private IEnumerator Timer(float time){ // 타이머 시간이 지나면 무승부
        if(isInGame){
            StartCoroutine(WhoWin());
        }
        yield return new WaitForSeconds(time);
        phase = false;
        if(isInGame){
            Tied();
        }
    }
    private IEnumerator WhoWin(){ // 누가 이겼는지 감지
        while(isInGame){
            if(currentMode == GameMode.KillAll){
                if(BluePlayer == 0){
                    UIManager.Instance.SetScoreText(blueWin: true);
                    isInGame = false;
                    phase = false;
                }
                if(RedPlayer == 0){
                    UIManager.Instance.SetScoreText(blueWin: false);
                    isInGame = false;
                    phase = false;
                }
            }
            if(currentMode == GameMode.TakePlace){
                if(UIManager.Instance.BlueTeamScore == 100){
                    UIManager.Instance.SetScoreText(blueWin: true);
                    isInGame = false;
                    phase = false;
                }
                if(UIManager.Instance.RedTeamScore == 100){
                    UIManager.Instance.SetScoreText(blueWin: false);
                    isInGame = false;
                    phase = false;
                }
            }
            yield return null;
        }
    }
    private void Tied(){ // 무승부 UI 띄우고 다시 게임 ㄱㄱ
        isInGame = false;
    }
    private void WinCheck(){
        if(UIManager.Instance.BlueTeamScore >= 2){
            game = false;
            // 만약 플레이어가 블루팀이면 승리 창 레드팀이면 패배창
        }
        if(UIManager.Instance.RedTeamScore >= 2){
            game = false;
            // 반대
        }
    }
}