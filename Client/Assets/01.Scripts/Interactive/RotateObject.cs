using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public void Rotate(float time)
    {
        StartCoroutine(Rotate90Coroutine(time));
    }
    
    private IEnumerator Rotate90Coroutine(float time)
    {
        //360을 넘어갔을 때 360 빼주기
        //360이 되는 순간 0도로 바꿔주는 처리
        //Quaternion originRotation = transform.rotation;
        
        float elapsedTime = 0.0f;

        Quaternion currentRotation = this.transform.rotation;
        Vector3 targetEulerAngles = this.transform.rotation.eulerAngles;
        targetEulerAngles.y += (89.0f);

        Quaternion targetRotation = Quaternion.Euler(targetEulerAngles);

        while (elapsedTime < time)
        {
            transform.rotation
                = Quaternion.Euler(Vector3.Lerp(
                    currentRotation.eulerAngles, targetRotation.eulerAngles, elapsedTime / time)
                );

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        targetEulerAngles.y = round(targetEulerAngles.y);
        this.transform.rotation = Quaternion.Euler(targetEulerAngles);
        

        yield return new WaitForSeconds(1f);
    }
    
    float round(float f)
    {
        float r = f % 90;
        return (r < 45) ? f - r : f - r + 90;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Rotate(1f);
        }
    }
}
