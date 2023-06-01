using System;
using System.Collections;
using UnityEngine;

public enum GameMode{ // 게임모드 임의로 지정함 점령은 스코어가 100이되면 이기고 섬멸은 다 죽여야 이김
    KillAll = 1,
    TakePlace = 2
}
public class Dohee_GameManager : MonoBehaviour
{
    private void DebugFunc(){
        if(Input.GetKeyDown(KeyCode.R)){
            phase = false;
            isInGame = false;
        }
        if(Input.GetKeyDown(KeyCode.F)){
            RedPlayer--;
        }
    }
    private void Update() {
        DebugFunc();
    }
    public static Dohee_GameManager Instance = null;
    private Coroutine currentRound = null;
    private GameMode currentMode = GameMode.KillAll;
    public GameMode CurrentMode => currentMode;
    private bool game; // 전체 게임의 값 매우 중요
    public bool Game => game;
    private bool phase; // 한칸 한칸 넘기는 값
    public bool Phase => phase;
    private bool isInGame; // 승리 계산하는지 확인하는 값
    public bool IsInGame => isInGame;
    private int redRoundScore = 0;
    private int blueRoundScore = 0;
    public int RedRoundScore => redRoundScore;
    public int BlueRoundScore => blueRoundScore;
    private int redModeScore = 0;
    private int blueModeScore = 0;
    public int RedModeScore => redModeScore;
    public int BlueModeScore => blueModeScore;
    [SerializeField]private int bluePlayer = 4;
    [SerializeField]private int redPlayer = 4;
    public int BluePlayer{
        get => bluePlayer;
        set{
            bluePlayer = Mathf.Clamp(value, 0, 5);
            if(currentMode == GameMode.KillAll) blueModeScore = bluePlayer;
            UIManager.Instance.SetModeScoreText(blueModeScore, redModeScore);
        }
    }
    public int RedPlayer{
        get => redPlayer;
        set{
            redPlayer = Mathf.Clamp(value, 0, 5);
            if(currentMode == GameMode.KillAll) redModeScore = redPlayer;
            UIManager.Instance.SetModeScoreText(blueModeScore, redModeScore);
        }
    }
    public event Action startGame; // 게임 시작 (로딩)
    public event Action endGame; // 게임 끝 (결과 창)
    public event Action waitRound; // 픽하는 라운드
    public event Action startRound; // 라운드 시작
    public event Action endRound; // 라운드 끝
    private void Awake(){
        if(Instance != null) Destroy(gameObject);
        Instance = this;
        DontDestroyOnLoad(gameObject);
        CreateManagers();
    }
    private void CreateManagers(){
        UIManager.Instance = new UIManager();
        UIManager.Instance.Init();
        TimerManager.Instance = new TimerManager();
        TimerManager.Instance.Init();
    }
    public void InGameSetting(){
        if(currentMode == GameMode.KillAll){
            redModeScore = redPlayer;
            blueModeScore = bluePlayer;
        }
        if(currentMode == GameMode.TakePlace){}
        game = true;
        UIManager.Instance.InGameLoad(currentMode);
        UIManager.Instance.SetModeScoreText(blueModeScore, redModeScore);
        UIManager.Instance.SetRoundScoreText(blueRoundScore, redRoundScore);
        StartCoroutine(StartGame());
    }
    private IEnumerator StartGame(){
        // phase = true;
        startGame?.Invoke();
        yield return new WaitUntil(() => !phase);
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
            GameWinCheck();
        }
        endGame?.Invoke();
    }
    public void TimerStart(float time){
        float currentTime = time;
        if(currentRound != null){
            StopCoroutine(currentRound);
        }
        currentRound = StartCoroutine(Timer(currentTime));
    }
    private IEnumerator Timer(float currentTime){ // 타이머 시간이 지나면 무승부
        while(currentTime >= 0 && phase){
            currentTime -= Time.deltaTime;
            UIManager.Instance.SetTimeText(currentTime);

            if(isInGame) RoundWinCheck();
            if(currentTime <= 0){
                phase = false;
                if(isInGame) Tied();
            }
            yield return null;
        }
    }
    private void RoundWinCheck(){
        if(currentMode == GameMode.KillAll){
            if(BluePlayer == 0){
                blueRoundScore++;
                UIManager.Instance.SetRoundScoreText(blueRoundScore, redRoundScore);
                BluePlayer = 4;
                RedPlayer = 4;
                isInGame = false;
                phase = false;
            }
            if(RedPlayer == 0){
                redRoundScore++;
                UIManager.Instance.SetRoundScoreText(blueRoundScore, redRoundScore);
                BluePlayer = 4;
                RedPlayer = 4;
                isInGame = false;
                phase = false;
            }
        }
        if(currentMode == GameMode.TakePlace){
            if(BlueModeScore >= 100){
                blueRoundScore++;
                UIManager.Instance.SetRoundScoreText(blueRoundScore, redRoundScore);
                BluePlayer = 4;
                RedPlayer = 4;
                isInGame = false;
                phase = false;
            }
            if(RedModeScore >= 100){
                redRoundScore++;
                UIManager.Instance.SetRoundScoreText(blueRoundScore, redRoundScore);
                BluePlayer = 4;
                RedPlayer = 4;
                isInGame = false;
                phase = false;
            }
        }
    }
    private void Tied(){ // 무승부 UI 띄우고 다시 게임 ㄱㄱ
        isInGame = false;
    }
    private void GameWinCheck(){
        if(BlueRoundScore >= 2){
            game = false;
        }
        if(RedRoundScore >= 2){
            game = false;
        }
    }
    public void ChangeGameMode(int mode){
        switch(mode){
            case 1: Dohee_GameManager.Instance.currentMode = GameMode.KillAll; break;
            case 2: Dohee_GameManager.Instance.currentMode = GameMode.TakePlace; break;
        }
    }
}