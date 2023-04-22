using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentCamera : MonoBehaviour
{
    private AgentInput _agentInput;

    [SerializeField] private float _sensitivity = 10f;
    private float _xRot = 0f;

    private void Awake()
    {
        _agentInput = transform.parent.GetComponent<AgentInput>();
        
        _agentInput.OnMousePosChange += CameraRotate;
    }

    private void CameraRotate(Vector2 mouseInput)
    {
        _xRot += mouseInput.y;
        transform.localRotation = Quaternion.Euler(_xRot * _sensitivity, 0, 0);
    }
}
