using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentInput : MonoBehaviour
{
    public event Action<Vector2> OnMousePosChange;
    public event Action<Vector3> OnMovementKeyPress;

    private Vector3 _dirInput;

    private void Update()
    {
        UpdateMousePos();
        UpdateMovementInput();
    }

    private void UpdateMousePos()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        OnMousePosChange?.Invoke(new Vector2(mouseX, mouseY));
    }

    private void UpdateMovementInput()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        _dirInput = new Vector3(h, 0, v);
        OnMovementKeyPress?.Invoke(_dirInput);
    }
}
