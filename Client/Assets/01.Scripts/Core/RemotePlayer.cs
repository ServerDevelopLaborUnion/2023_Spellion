using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemotePlayer : MonoBehaviour
{
    private Vector3 _targetPos;
    private Quaternion _targetRot;
    private float _lerpValue = 15f;

    public void SetPosAndRot(Vector3 pos, Quaternion rot, bool immediate = false)
    {
        if(immediate)
        {
            transform.position = pos;
            transform.rotation = rot;
        }
        else
        {
            _targetPos = pos;
            _targetRot = rot;
        }
    }

    private void Update()
    {
        Vector3 pos = Vector3.Lerp(transform.position, _targetPos, Time.deltaTime * _lerpValue);
        Quaternion rot = Quaternion.Lerp(transform.rotation, _targetRot, Time.deltaTime * _lerpValue);

        transform.position = pos;
    }
}
