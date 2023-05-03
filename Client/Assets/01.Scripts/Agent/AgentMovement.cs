using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentMovement : MonoBehaviour
{
    private AgentInput _agentInput;

    private Vector3 _movementVelocity;
    [SerializeField] private float _sensitivity;
    private float _yRot = 0f;

    private Transform _gunTrm;

    private void Awake()
    {
        _agentInput = GetComponent<AgentInput>();
        _gunTrm = transform.Find("Gun");

        _agentInput.OnMovementKeyPress += SetMovementVelocity;
        _agentInput.OnMousePosChange += AgentRotate;
    }

    private void SetMovementVelocity(Vector3 dir)
    {
        _movementVelocity = dir;
    }

    private void AgentRotate(Vector2 mouseInput)
    {
        _yRot += mouseInput.x;
        transform.rotation = Quaternion.Euler(0, _yRot * _sensitivity, 0);
    }

    private void FixedUpdate()
    {
        
    }
}
