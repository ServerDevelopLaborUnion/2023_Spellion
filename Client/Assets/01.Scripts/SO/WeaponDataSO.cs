using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/WeaponData")]
public class WeaponDataSO : ScriptableObject
{
    public string WeaponName;
    public float ActiveCoolDown;

    public int AttackDamage;
    public float Magazine;
}
