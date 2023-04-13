using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotater : MonoBehaviour
{
    private PlayerInput _playerInput;
    private float _eulerAngleX;
    private float _eulerAngleY;
    private float _rotSpeed = 5;
    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();

        //_playerInput.OnMovementKeyPress += SetBodyRotation;
        _playerInput.OnMouseMove += SetBodyRotation;
    }

    private void SetBodyRotation(float mouseX, float mouseY)
    {
        // if (dir.sqrMagnitude > 0)
        // {
        //     transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), 2 * Time.deltaTime);
        // }
        _eulerAngleY += mouseX * _rotSpeed;
        transform.rotation = Quaternion.Euler(0, _eulerAngleY, 0);
    }
}
