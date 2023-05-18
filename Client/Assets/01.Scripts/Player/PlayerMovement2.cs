using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement2 : MonoBehaviour
{
    [SerializeField] private PlayerPropertySO _playerPropertySO;
    private CharacterController _characterController;
    
    private float _gravity = -9.8f;
    
    private float _groundCheckRadius = 0.4f;
    public LayerMask GroundLayer;
    
    public bool _isGrounded;

    private Vector3 _verticalVelocity;
    private Vector3 _groundCheckPos;

    

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();

        
    }

    private void Update()
    {
        _groundCheckPos = new Vector3(transform.position.x, transform.position.y - 0.9f, transform.position.z);
        _isGrounded = Physics.CheckSphere(_groundCheckPos, _groundCheckRadius, GroundLayer);

        if (_isGrounded && _verticalVelocity.y < 0)
        {
            _verticalVelocity.y = -1.5f;
        }

        float hori = Input.GetAxis("Horizontal");
        float verti = Input.GetAxis("Vertical");

        Vector3 move = transform.right * hori + transform.forward * verti;
        move *= _playerPropertySO.MoveSpeed;

        _characterController.Move(move * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            _verticalVelocity.y = Mathf.Sqrt(_playerPropertySO.JumpForce * -2f * _gravity);
        }
        
        _verticalVelocity.y += _gravity * Time.deltaTime;
        _characterController.Move(_verticalVelocity * Time.deltaTime);

    }
}
