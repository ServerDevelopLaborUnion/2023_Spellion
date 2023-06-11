using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Packet;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

[RequireComponent(typeof(AgentInput), typeof(CharacterController))]
public class AgentMovement : MonoBehaviour
{
    // Component
    private AgentInput _agentInput;
    private CharacterController _charController;
    private Camera _mainCam;
    
    [SerializeField] private Transform _headTrm;
    [SerializeField, Range(1, 100)] private float _upperLookLimit = 80.0f, _lowerLookLimit = 80.0f;
    private float _rotationX = 0f;

    // Movement
    [SerializeField] private PlayerPropertySO _moveData;
    private Vector3 _movementVelocity;
    private float _verticalVelocity;
    private bool _isJump = false;

    private void Awake()
    {
        _agentInput = GetComponent<AgentInput>();
        _charController = GetComponent<CharacterController>();
        _mainCam = Camera.main;

        _agentInput.OnMovementKeyInput += SetMoveVelocity;
        _agentInput.OnJumpKeyPress += SetJump;
        _agentInput.OnMousePosInput += SetRotation;
    }

    private void SetJump()
    {
        if(_charController.isGrounded)
        _verticalVelocity += Mathf.Sqrt(_moveData.JumpForce * -3.0f * _moveData.Gravity);
    }

    private void SetRotation(Vector2 mouseInput)
    {
        _mainCam.transform.position = _headTrm.position;
        _rotationX -= mouseInput.x;
        _rotationX = Mathf.Clamp(_rotationX, -_upperLookLimit, _lowerLookLimit);
        _mainCam.transform.rotation = Quaternion.Euler(_rotationX, _mainCam.transform.eulerAngles.y, 0);

        float rotationY = mouseInput.y;
        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y + rotationY, 0);
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
        _movementVelocity = transform.rotation * _movementVelocity;
        _movementVelocity *= _moveData.MoveSpeed * Time.fixedDeltaTime;
    }

    private void FixedUpdate()
    {    
        CalculatePlayerMovement();
        if (_charController.isGrounded && _verticalVelocity < 0)
        {
            _verticalVelocity = 0f;
        }
        _charController.Move(_movementVelocity);

        _verticalVelocity += _moveData.Gravity * Time.fixedDeltaTime;
        _charController.Move((_movementVelocity + _verticalVelocity * Vector3.up) * Time.fixedDeltaTime);
    }
}
