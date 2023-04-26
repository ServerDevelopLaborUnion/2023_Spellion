using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDmageable
{
    private int startHp = 100;
    private int currentHp = 100;
    private bool isDie = false;
    private void Update()
    {
        if (currentHp <= 0)
            OnDie();

        if (isDie == true)
            Reset();
    }
    public void OnDamage(int damage)
    {
        currentHp -= damage;
    }
    public void OnHeal(int healAmount)
    {
        currentHp += healAmount;
    }
    private void OnDie()
    {
        isDie = true;
    }
    private void Reset()
    {
        currentHp = startHp;
        isDie = true;
    }
}
