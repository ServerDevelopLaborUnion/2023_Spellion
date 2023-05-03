using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] float _maxLength;
    [SerializeField] float _laserWidth;

    private LineRenderer _lineRenderer;
    private string _hitTag = "Enemy";
    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();

        _lineRenderer.startWidth = _laserWidth;
        _lineRenderer.endWidth = _laserWidth;
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

    void LaserSpawn()
    {
        _lineRenderer.enabled = true;

        if (Physics.Raycast(new(transform.position, transform.forward), out RaycastHit hit, _maxLength))
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
}