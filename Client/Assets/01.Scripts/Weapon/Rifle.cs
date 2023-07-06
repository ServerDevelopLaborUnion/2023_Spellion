using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Rifle : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup _hitSoundMixerGroup;
    [SerializeField] private string _wallTag = "Remote";
    [SerializeField] private string _hitTag = "Remote";
    [SerializeField] private float _rateOfFire = 0.1f;
    [SerializeField] private float _maxLength;
    [SerializeField] private int _MaxAmmo = 30;
    [SerializeField] private int _damage = 5;
    private AudioSource _audioSource;
    private Vector3 _dir;
    private int _currentAmmo = 100;
    private bool _shotAble = true;
    private bool _isGizmo = false;
    private bool _isAuto = false;
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _currentAmmo = _MaxAmmo;
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
        _shotAble = false;
        Debug.Log("Start Reloading");
        yield return new WaitForSeconds(0.7f);
        Debug.Log("End Reloading");
        _shotAble = true;
        _currentAmmo = _MaxAmmo;
    }
    void ShotCheck()
    {
        if(_shotAble && _currentAmmo > 0 && Input.GetMouseButtonDown(0)){
            _shotAble = false;
            StartCoroutine("Shot");
        }
        else if(_isAuto && _currentAmmo > 0 && Input.GetMouseButtonUp(0)){
            _shotAble = true;
            StopCoroutine("Shot");
        }
        else if(_shotAble && _currentAmmo <=0 && Input.GetMouseButtonDown(0))
            AudioManager.Instance.PlayAudio("NoBullet", _audioSource);
    }
    private void SetDir()
    {
        _dir = transform.forward * _maxLength;
    }
    IEnumerator Shot(){
        do{
            _currentAmmo--;
            Debug.Log(_currentAmmo);
            AudioManager.Instance.PlayAudio("Rifle", _audioSource);
            if (Physics.Raycast(new Ray(transform.position, _dir), out RaycastHit hit, _maxLength))
                {
                    if (hit.collider.tag == _hitTag)
                    {
                        hit.collider.GetComponent<PlayerHealth>().OnDamage(_damage);
                        MakeMark(hit, "BulletHitEnemy");
                    }
                    if (hit.collider.tag == _wallTag){
                        MakeMark(hit, "BulletHitWall");
                    }
                }
            StartCoroutine(DrawLine());
            yield return new WaitForSeconds(_rateOfFire);
        }while(_isAuto && _currentAmmo > 0);
        _shotAble = true;
    }
    private void MakeMark(RaycastHit hit, string sound){
        GameObject bulletMark = new GameObject("BulletMark");
        AudioSource audioSource = bulletMark.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = _hitSoundMixerGroup;
        bulletMark.transform.position = hit.point;
        bulletMark.transform.rotation = Quaternion.LookRotation(hit.transform.forward, hit.transform.up);
        AudioManager.Instance.PlayAudio(sound, audioSource);
        Destroy(bulletMark, 3f);
    }
    private void ChangeGunMode(){
        if(_isAuto){
            _isAuto = false;
            Debug.Log("single");
        }
        else{
            _isAuto = true;
            Debug.Log("auto");
        }
        AudioManager.Instance.PlayAudio("ChangeGunMode", _audioSource);
    }
    IEnumerator DrawLine()
    {
        _isGizmo = true;
        yield return new WaitForSeconds(0.08f);
        _isGizmo = false;
    }
    private void OnDrawGizmos()
    {
        if (_isGizmo == false)
            Gizmos.color = Color.clear;
        else
            Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, _dir * _maxLength);
    }
}
