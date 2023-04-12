using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private CameraRotate _cameraRotate;
    public event Action<Vector3> OnMovementKeyPress = null;
    public event Action OnJumpKeyPress = null;
    public event Action OnDuckingKeyPress = null;

    private Vector3 _moveInput;

    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        UpdateMoveInput();
        GetMouseDragInput();
    }

    private void UpdateMoveInput()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");  
        float vertical = Input.GetAxisRaw("Vertical");  
        _moveInput = new Vector3(horizontal, 0, vertical);
        OnMovementKeyPress?.Invoke(_moveInput);
    }
    private void GetMouseDragInput()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        
        _cameraRotate.UpdateRotate(mouseX, mouseY);
    }
}
