using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AgentInput), typeof(CharacterController))]
public class AgentMovement : MonoBehaviour
{
    // Component
    private AgentInput _agentInput;
    private CharacterController _charController;

    [SerializeField] private PlayerPropertySO _moveData;

    // Movement
    private Vector3 _movementVelocity;
    private float _gravity = -9.8f;
    private float _verticalVelocity;
    private bool _isJump = false;

    private void Awake()
    {
        _agentInput = GetComponent<AgentInput>();
        _charController = GetComponent<CharacterController>();

        _agentInput.OnMovementKeyInput += SetMoveVelocity;
        _agentInput.OnJumpKeyPress += SetJump;
    }

    private void SetJump()
    {
        _isJump = true;
    }

    private void SetMoveVelocity(Vector3 dir)
    {
        _movementVelocity = dir;
    }

    public void StopImmediately()
    {
        _movementVelocity = Vector3.zero;
    }

    private void CalculatePlayerMovement()
    {
        _movementVelocity.Normalize();
        _movementVelocity *= _moveData.MoveSpeed * Time.deltaTime;
    }

    private void FixedUpdate()
    {    
        CalculatePlayerMovement();
        if (_charController.isGrounded == false)
        {
            _verticalVelocity = _gravity * Time.fixedDeltaTime;
        }
        else
        {
            _verticalVelocity = _gravity * 0.3f * Time.fixedDeltaTime;
            if(_isJump)
            {
                _verticalVelocity *= -_moveData.JumpForce;
                _isJump = false;
            }
        }

        Vector3 move = _movementVelocity + _verticalVelocity * Vector3.up;
        _charController.Move(move);
    }
}
