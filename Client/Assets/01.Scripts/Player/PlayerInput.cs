using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public event Action<Vector3> OnMovementKeyPress = null;
    public event Action<float, float> OnMouseMove = null;
    public event Action OnJumpKeyPress = null;
    public event Action OnDuckingKeyPress = null;
    
    private Vector3 _moveInput;
    private Vector3 _mouseInput;

    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        UpdateMoveInput();
        UpdateKeyInput();
        GetMouseDragInput();
    }

    private void UpdateMoveInput()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");  //좌우 입력 => 입력값 * transform.right = 좌우 움직임 (보는 방향)
        float vertical = Input.GetAxisRaw("Vertical");  // 위아래 입력 => 입력값 * transform.forward = 앞뒤 움직임 (보는 방향)
        // _moveInput = new Vector3(horizontal, 0, vertical);
        _moveInput = (transform.forward * vertical) + (transform.right * horizontal);
        OnMovementKeyPress?.Invoke(_moveInput);
    }

    private void UpdateKeyInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnJumpKeyPress?.Invoke();
        }
    }
    private void GetMouseDragInput()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        
        OnMouseMove?.Invoke(mouseX, mouseY);
    }
}
