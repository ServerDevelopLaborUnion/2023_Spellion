using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public WeaponDataSO WeaponSO;
    
    private float _currentTimer;

    public abstract void ActiveWeapon();
    public abstract void ActiveEffect();
    public abstract IEnumerator CoolDownCoroutine();


}
