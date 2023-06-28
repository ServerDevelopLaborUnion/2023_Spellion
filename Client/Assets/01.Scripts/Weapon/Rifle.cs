using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : MonoBehaviour
{
    [SerializeField] float _maxLength;
    [SerializeField] private string _hitTag = "Remote";
    [SerializeField] float rateOfFire = 0.1f;
    [SerializeField] int MaxAmmo = 30;
    [SerializeField] int damage = 5;
    [SerializeField] GameObject player;
    AudioSource audioSource;
    Vector3 dir;
    int currentAmmo = 100;
    bool shotAble = true;
    bool isGizmo = false;
    private bool gunMode = false;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        currentAmmo = MaxAmmo;
        Debug.Log("�ܹ߸��");
    }
    // false = 단발 / true = 연사
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R)) StartCoroutine(Reload());
        if(shotAble) IsGunMode();
        SetDir();
    }
    IEnumerator Reload()
    {
        shotAble = false;
        Debug.Log("Start Reloading");
        yield return new WaitForSeconds(0.7f);
        Debug.Log("End Reloading");
        shotAble = true;
        currentAmmo = MaxAmmo;
    }
    void IsGunMode()
    {
        if(Input.GetKeyDown(KeyCode.V))
            ChangeGunMode();
        if (gunMode == false && Input.GetMouseButtonDown(0) && currentAmmo > 0)
            SingleShot();
        if (gunMode == true && Input.GetMouseButtonDown(0) && currentAmmo > 0)
            StartCoroutine("Repeater");
        if (gunMode == true && Input.GetMouseButtonUp(0) || currentAmmo <= 0)
            StopCoroutine("Repeater");
        if(currentAmmo <= 0 && Input.GetMouseButtonUp(0))
            AudioManager.Instance.PlayAudio("NoBullet", audioSource);
    }
    private void SetDir()
    {
        Vector3 a = player.transform.forward * _maxLength;
        a.y = transform.position.y;
        dir = (a - transform.position).normalized;
    }
    void SingleShot()
    {
        currentAmmo--;
        Debug.Log(currentAmmo);
        AudioManager.Instance.PlayAudio("Rifle", audioSource);
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
            AudioManager.Instance.PlayAudio("Rifle", audioSource);
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
    private void ChangeGunMode(){
        if(gunMode){
            gunMode = false;
            Debug.Log("single");
        }
        else{
            gunMode = true;
            Debug.Log("auto");
        }
        AudioManager.Instance.PlayAudio("ChangeGunMode", audioSource);
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
