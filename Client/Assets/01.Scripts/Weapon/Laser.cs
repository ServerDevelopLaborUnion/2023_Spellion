using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] float _maxLength;
    [SerializeField] float _laserWidth;

    LineRenderer lineRenderer;
    private string hitTag = "Enemy";
    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.startWidth = _laserWidth;
        lineRenderer.endWidth = _laserWidth;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            LaserSpawn();
        }
        else
            lineRenderer.enabled = false;
    }

    void LaserSpawn()
    {
        lineRenderer.enabled = true;

        if (Physics.Raycast(new(transform.position, transform.forward), out RaycastHit hit, _maxLength))
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, hit.point);

            if (hit.collider.tag == hitTag)
            {
                Debug.Log("맞았음!");
            }
        }
        else
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, transform.position + transform.forward * _maxLength);
        }
    }
}