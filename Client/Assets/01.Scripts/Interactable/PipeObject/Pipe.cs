using Cinemachine;
using UnityEngine;
using System;

public class Pipe : Interactable
{
    CinemachinePath _path;

    public override void OnEnterInteractive()
    {
        Array.Reverse(_path.m_Waypoints);
    }
}
