using System.Collections;
using System.Collections.Generic;
using Kinemation.FPSFramework.Runtime.Core.Components;
using UnityEngine;

public class CInemaCam : MonoBehaviour
{
    public void EndOfCam()
    {
        GameManager.Instance.PlayerCam.gameObject.SetActive(true);
        Camera.SetupCurrent(GameManager.Instance.PlayerCam);
        GameManager.Instance.Player.enabled = true;
        GameManager.Instance.Player.GetComponentInChildren<CoreAnimComponent>().enabled = true;
        Destroy(gameObject);
        Debug.Log("Cam Change");
    }
}
