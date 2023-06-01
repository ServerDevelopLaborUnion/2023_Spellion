using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class ObjectCasting : MonoBehaviour
{
    [SerializeField] private float _maxDistance;
    [SerializeField] private LayerMask _interactableLayer;

    private MeshRenderer _meshRenderer;

    public UnityEvent OnAimPopup;
    public UnityEvent OnAimPopdown;

    private bool isInteractive;

    private InterUI _interUI;

    private void Awake()
    {
        _interUI = GameObject.Find("UIDocument").GetComponent<InterUI>();
        OnAimPopup.AddListener(_interUI.AddOnClass);
        OnAimPopdown.AddListener(_interUI.RemoveOnClass);
    }

    private void Update()
    {
        RaycastHit hit;
        bool isHit = Physics.Raycast(transform.position, transform.forward, out hit, _maxDistance, _interactableLayer);
        
        if (isHit)
        {
            //if (_meshRenderer.materials[1].name == "ObjectOutlineMat")
            if (isInteractive == false)
            {
                isInteractive = true;
                _meshRenderer = hit.collider.gameObject.GetComponent<MeshRenderer>();
                _meshRenderer.materials[1].SetInt("_OutLine", 1);
                OnAimPopup?.Invoke();
            }
        }
        else
        {
            if (isInteractive == true)
            {
                _meshRenderer.materials[1].SetInt("_OutLine", 0);
                isInteractive = false;
                OnAimPopdown?.Invoke();
            }

        }
    }
}
