using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] float _maxLength;
    [SerializeField] float _laserWidth;

    private LineRenderer _lineRenderer;
    [SerializeField] private string _hitTag = "Remote";
    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();

        _lineRenderer.startWidth = _laserWidth;
        _lineRenderer.endWidth = _laserWidth;

        // GetComponentInParent<PlayerInput>().OnFireKeyPress += LaserSpawn;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            LaserSpawn();
        }
        else
            _lineRenderer.enabled = false;
    }

    private void LaserSpawn()
    {
        _lineRenderer.enabled = true;

        if (Physics.Raycast(new Ray(transform.position, transform.forward), out RaycastHit hit, _maxLength))
        {
            _lineRenderer.SetPosition(0, transform.position);
            _lineRenderer.SetPosition(1, hit.point);

            if (hit.collider.tag == _hitTag)
            {
                Debug.Log("맞았음!");
            }
        }
        else
        {
            _lineRenderer.SetPosition(0, transform.position);
            _lineRenderer.SetPosition(1, transform.position + transform.forward * _maxLength);
        }
    }

    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Color oldColor = Gizmos.color;
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, transform.position + transform.forward * _maxLength);
        Gizmos.color = oldColor;
    }
    #endif
}