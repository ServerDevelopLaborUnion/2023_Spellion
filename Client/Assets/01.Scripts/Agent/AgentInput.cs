using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentInput : MonoBehaviour
{
    // Movement
    public event Action<Vector3> OnMovementKeyInput;
    public event Action OnJumpKeyPress;

    // Fire
    public event Action OnFireKeyPress;
    public event Action OnFireKeyRelease;

    private void Update()
    {
        UpdateMovementInput();
        UpdateFireKeyInput();
    }

    private void UpdateFireKeyInput()
    {
        if(Input.GetMouseButtonDown(0))
        {
            OnFireKeyPress?.Invoke();
        }
        if(Input.GetMouseButtonUp(0))
        {
            OnFireKeyRelease?.Invoke();
        }
    }

    private void UpdateMovementInput()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        OnMovementKeyInput?.Invoke(new Vector3(x, 0, z));

        if(Input.GetButtonDown("Jump"))
        {
            OnJumpKeyPress?.Invoke();
        }
    }
}
