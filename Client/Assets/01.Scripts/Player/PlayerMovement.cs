using System;
using System.Collections;
using System.Collections.Generic;
using Packet;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerPropertySO _playerPropertySO;
    private float _gravity = -9.8f;
    
    private CharacterController _characterController;
    private PlayerInput _playerInput;

    private float _groundCheckDistance = 0.4f;
    public LayerMask GroundLayer;
    
    bool _isGrounded;
    private Vector3 _movementVelocity;

    public bool CanMove = true;
    public bool CanJump = false;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _playerInput = GetComponent<PlayerInput>();
        

        _playerInput.OnMovementKeyPress += CalcAcceleration;
        //_playerInput.OnMovementKeyPress += Move;
        _playerInput.OnJumpKeyPress += Jump;
        _playerInput.OnDuckingKeyPress += Ducking;
    }

    private void CalcAcceleration(Vector3 inputVelocity)    
    {
        //inputVelocity.Normalize();
        _movementVelocity = inputVelocity * (_playerPropertySO.MoveSpeed);
    }

    private void StopMove()
    {
        _movementVelocity = Vector3.zero;
    }
    
    private void Update() 
    {
        _isGrounded = Physics.CheckSphere(gameObject.transform.position, _groundCheckDistance, GroundLayer);
        
        if(_isGrounded && _movementVelocity.y < 0)
        {
            _movementVelocity.y = -2f;
        }
        
        _characterController.Move(_movementVelocity * Time.deltaTime);
        
        if(Input.GetButtonDown("Jump") && _isGrounded)
        {
            _movementVelocity.y = Mathf.Sqrt(_playerPropertySO.JumpForce * -2f * _gravity);
        }
        
        _movementVelocity.y += _gravity * Time.deltaTime;

        _characterController.Move(_movementVelocity * Time.deltaTime);
    }
    

    private void Jump()
    {
        Debug.Log("점프");
        //_rigidbody.AddForce(Vector3.up * _playerPropertySO.JumpForce, ForceMode.Impulse);
    }

    private void Ducking()
    {
        
    }
}
