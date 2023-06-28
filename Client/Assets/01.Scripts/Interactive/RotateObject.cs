using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    private bool _isMoving = true; 
    public void Rotate(float time)
    {
        Debug.Log("Rotate");
        StartCoroutine(Rotate90Coroutine(time));
    }
    
    private IEnumerator Rotate90Coroutine(float time)
    {
        _isMoving = true;

        if (_isMoving)
        {
            Debug.Log("Coroutine");
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

            _isMoving = false;
        }
            yield return new WaitForSeconds(1f);
    }

    float round(float f)
    {
        float r = f % 90;
        return (r < 45) ? f - r : f - r + 90;
    }
}
