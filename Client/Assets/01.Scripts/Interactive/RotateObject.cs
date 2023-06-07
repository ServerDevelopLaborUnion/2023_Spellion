using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public void Rotate(float time, float angle, bool clockWise = true)
    {
        StartCoroutine(RotateCoroutine(time, angle));
    }
    
    private IEnumerator RotateCoroutine(float time, float angle, bool clockWise = true)
    {
        //360을 넘어갔을 때 360 빼주기
        //360이 되는 순간 0도로 바꿔주는 처리

        

        yield return new WaitForSeconds(1f);
    }
    
    // float round(float f, float angle)
    // {
    //     float r = f % angle;
    //     return (r < angle / 2) ? f - r : f - r + angle;
    // }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Rotate(1f, 90f);
        }
    }
}
