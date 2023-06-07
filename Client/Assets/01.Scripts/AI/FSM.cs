using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] float detectionRange = 10f;
    [SerializeField] float attackRange = 5f;
    [SerializeField] int damage = 5;
    [SerializeField] Collider myself;

    Collider target;
    Vector3 targetDir;
    float delayTime = 0.1f;
    float span = 0;
    bool isDetect = false;
    bool isGizmo = false;
    public enum State
    {
        WALK,
        DETECT,
        ATTACK
    }
    public State state = State.WALK;

    private void Update()
    {
        switch (state)
        {
            case State.WALK:
                Walk();
                break;
            case State.DETECT:
                Detect();
                break;
            case State.ATTACK:
                Attack();
                break;
        }
    }

    private void ChangeState(State state)
    {
        //스테이트에서 나가기전에 마지막으로 실행되는 Exit()함수;
        switch (this.state)
        {
            case State.WALK:
                WalkExit();
                break;
            case State.DETECT:
                DetectExit();
                break;
            case State.ATTACK:
                AttackExit();
                break;
        }

        this.state = state;

        //스테이트에서 들어가고나서 처음으로 실행되는 Enter()함수;
        switch (state)
        {
            case State.WALK:
                WalkEnter();
                break;
            case State.DETECT:
                DetectEnter();
                break;
            case State.ATTACK:
                AttackEnter();
                break;
        }
    }

    private void WalkEnter()
    {
        target = null;
    }
    private void Walk()
    {
        Debug.Log("걷는 중");
        foreach (RaycastHit hit in Physics.SphereCastAll(transform.position, detectionRange, Vector3.up, 0))
        {
            if (hit.collider != myself)
            {
                if (hit.collider.CompareTag("Player"))
                {
                    target = hit.collider;
                    Debug.Log(target);
                    ChangeState(State.DETECT);
                    break;
                }
            }
        }
    }
    private void WalkExit()
    {
        
    }

    private void DetectEnter()
    {
        
    }
    public void Detect()
    {
        isDetect = false;
        transform.position += (target.transform.position - transform.position).normalized * speed * Time.deltaTime;
        foreach (RaycastHit hit in Physics.SphereCastAll(transform.position, attackRange, Vector3.up, 0))
        {
            if (hit.collider != myself)
            {
                isDetect = true;
                Debug.Log(hit.collider);
                if (hit.collider.CompareTag("Player"))
                {
                    ChangeState(State.ATTACK);
                }
            }
        }
        if (isDetect == false)
        {
            ChangeState(State.WALK);
        }
    }
    private void DetectExit()
    {

    }

    private void AttackEnter()
    {
        
    }
    private void Attack()
    {
        targetDir = (target.transform.position - transform.position).normalized;
        span += Time.deltaTime;
        if (span > 0.08f) isGizmo = false;
        if (span > delayTime)
        {
            isDetect = false;
            if (Physics.Raycast(new Ray(transform.position, targetDir), out RaycastHit hit, attackRange))
            {
                if (hit.collider != myself)
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        //hit.collider.GetComponent<PlayerHealth>().OnDamage(damage);
                        isGizmo = true;
                    }
                }
            }
            span = 0;
        }
        foreach (RaycastHit check in Physics.SphereCastAll(transform.position, attackRange, Vector3.up, 0))
        {
            if (check.collider != myself) isDetect = true;
        }
        if(isDetect == false)
        {
            ChangeState(State.DETECT);
        }
    }
    private void AttackExit()
    {
        isGizmo = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        if(isGizmo == false)
            Gizmos.color = Color.clear;
        if (isGizmo == true)
            Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, targetDir * attackRange);
    }
}