using System;
using System.Collections;
using System.Collections.Generic;
using Packet;
using TMPro;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class RemotePlayer : PoolableMono
{
    private PlayerInfo _playerInfo;

    private LineRenderer _lineRenderer;

    private TextMeshPro _uuidText;

    [SerializeField] private float _laserWidth = 0.05f, _maxLength = 10f;
    [SerializeField] private string _hitTag;
    [SerializeField] private Transform _fireTrm;

    private void Awake()
    {
        _lineRenderer = transform.Find("Gun").GetComponentInChildren<LineRenderer>();
        _uuidText = transform.Find("UUID").GetComponent<TextMeshPro>();
        _fireTrm = _lineRenderer.transform;
    }

    public override void Reset()
    {
        _playerInfo = null;
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        
        _lineRenderer.enabled = false;
        _lineRenderer.startWidth = _laserWidth;
        _lineRenderer.endWidth = _laserWidth;

        _uuidText.SetText("");
        transform.SetParent(RemoteManager.Instance.transform);
    }

    public void Init(PlayerInfo playerInfo)
    {
        _playerInfo = playerInfo;

        transform.position = new Vector3(_playerInfo.Pos.X, _playerInfo.Pos.Y, _playerInfo.Pos.Z);
        transform.rotation = Quaternion.Euler(0, _playerInfo.Rot.Y, 0);

        _lineRenderer.positionCount = 2;

        _uuidText.SetText(playerInfo.Uuid);

        transform.SetParent(null);
    }

    public void SetPosAndRot(Packet.Vector3 pos, Packet.Vector2 rot)
    {
        transform.position = new Vector3(pos.X, pos.Y, pos.Z);
        transform.rotation = Quaternion.Euler(0, rot.Y, 0);
    }

    public void StartFire()
    {
        StartCoroutine(LaserLoop());
    }

    private IEnumerator LaserLoop()
    {
        _lineRenderer.enabled = true;
        while(true)
        {
            if (Physics.Raycast(new Ray(_fireTrm.position, _fireTrm.forward), out RaycastHit hit, _maxLength))
            {
                _lineRenderer.SetPosition(0, _fireTrm.position);
                _lineRenderer.SetPosition(1, hit.point);

                if (hit.collider.CompareTag(_hitTag))
                {
                    Debug.Log("맞았음!");
                }
            }
            else
            {
                _lineRenderer.SetPosition(0, _fireTrm.position);
                _lineRenderer.SetPosition(1, _fireTrm.position + _fireTrm.forward * _maxLength);
            }
            yield return null;
        }
    }

    public void StopFire()
    {
        _lineRenderer.enabled = false;
        StopAllCoroutines();
    }

    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Color oldColor = Gizmos.color;
        Gizmos.color = Color.green;
        Gizmos.DrawRay(_fireTrm.position, _fireTrm.position + _fireTrm.forward * _maxLength);
        Gizmos.color = oldColor;
    }
    #endif
}
