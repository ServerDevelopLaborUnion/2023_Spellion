using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentMovementSounds : MonoBehaviour
{
    [SerializeField] private AgentInput _agentInput;
    [SerializeField] private AgentMovement _agentMovement;
    private AudioSource _audioSource;
    private bool wasGrounded;
    private bool walkAble = false;
    private void Awake() {
        _audioSource = GetComponent<AudioSource>();
        _agentInput.OnJumpKeyPress -= Jump;
        _agentInput.OnJumpKeyPress += Jump;
        wasGrounded = _agentMovement.CharController.isGrounded;
    }
    private void Update() {
        if(_agentMovement.CharController.isGrounded && _agentMovement.MovementVelocity != Vector3.zero && walkAble)
            StartCoroutine("FootSteps");
        if(!wasGrounded && _agentMovement.CharController.isGrounded)
            Landing();
        wasGrounded = _agentMovement.CharController.isGrounded;
    }
    private IEnumerator FootSteps(){
        walkAble = false;
        AudioManager.Instance.PlayAudio("FootSteps", _audioSource);
        yield return new WaitForSeconds(0.4f);
        walkAble = true;
    }
    private void Landing(){
        AudioManager.Instance.PlayAudio("LandingSound", _audioSource);
        StopCoroutine("FootSteps");
        Debug.Log("Land");
        walkAble = true;
    }
    private void Jump(){
        AudioManager.Instance.PlayAudio("JumpSound", _audioSource);
        StopCoroutine("FootSteps");
        Debug.Log("Jump");
        walkAble = false;
    }
}
