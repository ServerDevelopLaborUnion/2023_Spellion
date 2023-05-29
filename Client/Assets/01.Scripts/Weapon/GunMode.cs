using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunMode : MonoBehaviour
{
    [SerializeField] float _maxLength;
    [SerializeField] private string _hitTag = "Remote";
    [SerializeField] float rateOfFire = 0.1f;
    [SerializeField] int startAmmo = 100;
    [SerializeField] int damage = 5;
    [SerializeField] GameObject player;
    Vector3 dir;
    int currentAmmo = 100;
    bool isGizmo = false;
    private bool gunMode = false;
    private void Start()
    {
        currentAmmo = startAmmo;
        Debug.Log("단발모드");
    }
    // false = 단발모드 / true = 연사모드
    private void Update()
    {
        IsGunMode();
        Reload();
        SetDir();
    }
    void Reload()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            currentAmmo = startAmmo;
            Debug.Log(currentAmmo);
        }
    }
    void IsGunMode()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            Debug.Log("연사모드");
            gunMode = true;
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("단발모드");
            gunMode = false;
        }
        if (gunMode == false && Input.GetMouseButtonDown(0) && currentAmmo > 0)
            SingleShot();
        if (gunMode == true && Input.GetMouseButtonDown(0) && currentAmmo > 0)
            StartCoroutine("Repeater");
        if ((gunMode == true && Input.GetMouseButtonUp(0)) || currentAmmo <= 0)
            StopCoroutine("Repeater");
    }
    private void SetDir()
    {
        Vector3 a = player.transform.forward * _maxLength;
        a.y = transform.position.y;
        dir = a - transform.position;
        dir = dir.normalized;
    }
    void SingleShot()
    {
        currentAmmo--;
        Debug.Log(currentAmmo);
        if (Physics.Raycast(new Ray(transform.position, dir), out RaycastHit hit, _maxLength))
        {
            if (hit.collider.tag == _hitTag)
            {
                hit.collider.GetComponent<PlayerHealth>().OnDamage(damage);
            }
        }
        StartCoroutine("DrawLine");
    }
    IEnumerator Repeater()
    {
        while (true)
        {
            if (Physics.Raycast(new Ray(transform.position, dir), out RaycastHit hit, _maxLength))
            {
                if (hit.collider.tag == _hitTag)
                {
                    hit.collider.GetComponent<PlayerHealth>().OnDamage(damage);
                }
            }
            StartCoroutine("DrawLine");
            currentAmmo--;
            Debug.Log(currentAmmo);
            yield return new WaitForSeconds(rateOfFire);
        }
    }
    IEnumerator DrawLine()
    {
        isGizmo = true;
        yield return new WaitForSeconds(0.08f);
        isGizmo = false;
    }
    private void OnDrawGizmos()
    {
        if (isGizmo == false)
            Gizmos.color = Color.clear;
        else
            Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, dir * _maxLength);
    }
}
