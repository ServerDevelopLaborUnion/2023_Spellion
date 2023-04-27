using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerPropertySO _playerPropertySO;
    
    private float _gravity = -9.8f;
    private CharacterController _characterController;
    private PlayerInput _playerInput;

    private Vector3 _movementVelocity;
    private float _verticalVelocity;

    public bool CanMove = true;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _playerInput = GetComponent<PlayerInput>();

        _playerInput.OnMovementKeyPress += CalcAcceleration;
        _playerInput.OnJumpKeyPress += Jump;
        _playerInput.OnDuckingKeyPress += Ducking;
    }

    private void CalcAcceleration(Vector3 _inputVelocity)
    {
        _inputVelocity.Normalize();
        _movementVelocity = _inputVelocity * (_playerPropertySO.MoveSpeed * Time.fixedDeltaTime);
        
        // if(_movementVelocity.sqrMagnitude > 0)
        // {
        //     transform.rotation = Quaternion.LookRotation(_movementVelocity);
        // }
    }

    private void StopMove()
    {
        _movementVelocity = Vector3.zero;
    }
    
    private void FixedUpdate() 
    {
        if (CanMove)
        {
            Vector3 move = _movementVelocity + _verticalVelocity * Vector3.up;
            _characterController.Move(move);
            //_characterController.Move(_movementVelocity);
        }
        
        if(_characterController.isGrounded == false)
        {
            _verticalVelocity = _gravity * Time.fixedDeltaTime;
        }
        else
        {
            _verticalVelocity = _gravity * 0.3f * Time.fixedDeltaTime;
        }
    }
    

    private void Jump()
    {
        
    }

    private void Ducking()
    {
        
    }
}
