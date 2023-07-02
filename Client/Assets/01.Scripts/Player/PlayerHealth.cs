using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDmageable
{
    private AudioSource _audioSource;
    private int startHp = 100;
    private int currentHp = 100;
    private bool isDie = false;
    private bool _hurtSoundAble = true;
    private void Awake() {
        _audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (currentHp <= 0)
            OnDie();

        if (isDie == true)
            Reset();

        // testing key
        if(Input.GetKeyDown(KeyCode.H)) OnDamage(40);
    }
    public void OnDamage(int damage)
    {
        currentHp -= damage;
        if(_hurtSoundAble && currentHp > 0) StartCoroutine(HurtSound());
    }
    private IEnumerator HurtSound(){
        _hurtSoundAble = false;
        int rand = Random.Range(1, 3);
        AudioManager.Instance.PlayAudio("Hurt" + rand.ToString(), _audioSource);
        yield return new WaitForSeconds(2f);
        _hurtSoundAble = true;
    }
    public void OnHeal(int healAmount)
    {
        currentHp += healAmount;
    }
    private void OnDie()
    {
        isDie = true;
        AudioManager.Instance.PlayAudio("DieSound", _audioSource);
    }
    private void Reset()
    {
        currentHp = startHp;
        isDie = false;
    }
}
