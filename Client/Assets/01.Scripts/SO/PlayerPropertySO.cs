using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Player")]
public class PlayerPropertySO : ScriptableObject
{
    public float MoveSpeed;
    public float JumpForce;
    public float Gravity = -9.81f;

}
