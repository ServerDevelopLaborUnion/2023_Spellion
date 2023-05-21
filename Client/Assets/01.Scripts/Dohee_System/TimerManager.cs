using System;
using System.Collections;
using UnityEngine;

public class TimerManager
{
    public static TimerManager Instance;
    [SerializeField] float waitRoundTime = 10f;
    [SerializeField] float inRoundTime = 180f;
    [SerializeField] float endRoundTime = 5f;
    private void Awake() {
        Dohee_GameManager.Instance.waitRound += WaitRoundTimeSet;

        Dohee_GameManager.Instance.startRound += InRoundTimeSet;
        
        Dohee_GameManager.Instance.endRound += EndRoundTimeSet;
    }
    private void WaitRoundTimeSet(){
        Dohee_GameManager.Instance.TimerStart(waitRoundTime);
    }
    private void InRoundTimeSet(){
        Dohee_GameManager.Instance.TimerStart(inRoundTime);
    }
    private void EndRoundTimeSet(){
        Dohee_GameManager.Instance.TimerStart(endRoundTime);
    }
}