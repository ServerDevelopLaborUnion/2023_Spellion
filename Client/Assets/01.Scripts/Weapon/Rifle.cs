using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : MonoBehaviour
{
    [SerializeField] private string _wallTag = "Remote";
    [SerializeField] private string _hitTag = "Remote";
    [SerializeField] float rateOfFire = 0.1f;
    [SerializeField] float _maxLength;
    [SerializeField] int MaxAmmo = 30;
    [SerializeField] int damage = 5;
    AudioSource audioSource;
    Vector3 dir;
    int currentAmmo = 100;
    bool shotAble = true;
    bool isGizmo = false;
    private bool isAuto = false;
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
        if(Input.GetKeyDown(KeyCode.V)) ChangeGunMode();
        ShotCheck();
        SetDir();
    }
    IEnumerator Reload()
    {
        StopCoroutine("shot");
        shotAble = false;
        Debug.Log("Start Reloading");
        yield return new WaitForSeconds(0.7f);
        Debug.Log("End Reloading");
        shotAble = true;
        currentAmmo = MaxAmmo;
    }
    void ShotCheck()
    {
        if(shotAble && currentAmmo > 0 && Input.GetMouseButtonDown(0)){
            shotAble = false;
            StartCoroutine("Shot");
        }
        else if(isAuto && currentAmmo > 0 && Input.GetMouseButtonUp(0)){
            shotAble = true;
            StopCoroutine("Shot");
        }
        else if(shotAble && currentAmmo <=0 && Input.GetMouseButtonDown(0))
            AudioManager.Instance.PlayAudio("NoBullet", audioSource);
    }
    private void SetDir()
    {
        dir = transform.forward * _maxLength;
    }
    IEnumerator Shot(){
        do{
            currentAmmo--;
            Debug.Log(currentAmmo);
            AudioManager.Instance.PlayAudio("Rifle", audioSource);
            if (Physics.Raycast(new Ray(transform.position, dir), out RaycastHit hit, _maxLength))
                {
                    if (hit.collider.tag == _hitTag)
                    {
                        hit.collider.GetComponent<PlayerHealth>().OnDamage(damage);
                        // MakeMark(hit, "BulletHitEnemy"); 사운드 구하지 못함
                    }
                    if (hit.collider.tag == _wallTag){
                        MakeMark(hit, "BulletHitWall");
                    }
                }
            StartCoroutine(DrawLine());
            yield return new WaitForSeconds(rateOfFire);
        }while(isAuto && currentAmmo > 0);
        shotAble = true;
    }
    private void MakeMark(RaycastHit hit, string sound){
        GameObject bulletMark = new GameObject("BulletMark");
        AudioSource audioSource = bulletMark.AddComponent<AudioSource>();
        bulletMark.transform.position = hit.point;
        bulletMark.transform.rotation = Quaternion.LookRotation(hit.transform.forward, hit.transform.up);
        AudioManager.Instance.PlayAudio(sound, audioSource);
        Destroy(bulletMark, 3f);
    }
    private void ChangeGunMode(){
        if(isAuto){
            isAuto = false;
            Debug.Log("single");
        }
        else{
            isAuto = true;
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
