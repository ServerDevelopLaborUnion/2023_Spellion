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
        Vector3 wise;
        if (clockWise == true) wise = Vector3.up;
        else wise = Vector3.down;
        
        float elapsedTime = 0.0f;
        Quaternion currentRotation = transform.rotation;
        Vector3 targetEulerAngles = transform.rotation.eulerAngles;
        targetEulerAngles.y += angle * wise.y;
        
        Quaternion targetRotation = Quaternion.Euler(targetEulerAngles);
        
        while (elapsedTime < time)
        {
            transform.rotation = Quaternion.Euler(Vector3.Lerp(
                currentRotation.eulerAngles, targetRotation.eulerAngles, elapsedTime / time)
            );


            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        targetEulerAngles.y = round(targetEulerAngles.y, angle);
        transform.rotation = Quaternion.Euler(targetEulerAngles);

        yield return new WaitForSeconds(1f);
    }
    
    float round(float f, float angle)
    {
        float r = f % angle;
        return (r < angle / 2) ? f - r : f - r + angle;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Rotate(1f, 90f);
        }
    }
}
