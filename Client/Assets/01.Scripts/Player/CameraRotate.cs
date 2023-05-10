using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    [SerializeField]
    private float rotCamXAxisSpeed = 5;      
    [SerializeField]
    private float rotCamYAxisSpeed = 3;      

    private float limitMin = -80;          
    private float limitMax = 50;            
    private float eulerAngleX;
    private float eulerAngleY;

    private PlayerInput _playerInput;

    private Transform _gunTrm;

    private void Awake()
    {
        _playerInput = GetComponentInParent<PlayerInput>();
        _playerInput.OnMouseMove += UpdateRotate;
        _gunTrm = transform.parent.Find("Gun");
    }

    public void UpdateRotate(float mouseX, float mouseY)
    {
        //eulerAngleY += mouseX * rotCamYAxisSpeed;    // 마우스 좌/우 이동으로 카메라 y축 회전
        eulerAngleX -= mouseY * rotCamXAxisSpeed;    // 마우스 위/아래 이동으로 카메라 x축 회전
        
        eulerAngleX = ClampAngle(eulerAngleX, limitMin, limitMax);
        
        transform.localRotation = Quaternion.Euler(eulerAngleX, 0, 0);
        _gunTrm.localRotation = Quaternion.Euler(eulerAngleX - 5, 0, 0);
    }

    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360) angle += 360;
        if (angle > 360) angle -= 360;

        return Mathf.Clamp(angle, min, max);
    }
}
