using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotater : MonoBehaviour
{
    private PlayerInput _playerInput;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();

        _playerInput.OnMovementKeyPress += SetBodyRotation;
    }

    private void SetBodyRotation(Vector3 dir)
    {
        if (dir.sqrMagnitude > 0)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), 2 * Time.deltaTime);
        }
    }
}
