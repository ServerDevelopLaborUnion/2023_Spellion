using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class ObjectCasting : MonoBehaviour
{
    [SerializeField] private float _maxDistance;
    [SerializeField] private LayerMask _interactableLayer;

    private MeshRenderer _meshRenderer;

    public UnityEvent OnAimPopup;
    public UnityEvent OnAimPopdown;

    private bool _isRenderOutLine = false;
    private bool _canEnterInteractive;

    private InterUI _interUI;
    private RaycastHit hit;
    private void Awake()
    {
        _interUI = GameObject.Find("UIDocument").GetComponent<InterUI>();
        OnAimPopup.AddListener(_interUI.AddOnClass);
        OnAimPopdown.AddListener(_interUI.RemoveOnClass);
    }

    private void Update()
    {   
        bool isHit = Physics.Raycast(transform.position, transform.forward, out hit, _maxDistance, _interactableLayer);
        
        if (isHit) // interactable 오브젝트에 맞았을 때
        {
            if (_isRenderOutLine == false) //현재 아웃라인 렌더 상태가 아니라면
            {
                _isRenderOutLine = true; // 아웃라인 렌더중 상태로 바꾸고
                OutlineRender(); // 아웃라인을 렌더링한다.
                OnAimPopup?.Invoke(); // 상호작용 UI를 띄워주고
                _canEnterInteractive = true; // 상호작용 가능 상태로 만든다
            }
        }
        else // interactable 오브젝트에 맞지 않았을 때
        {
            if (_isRenderOutLine == true) // 아웃라인 렌더중이라면
            {
                _meshRenderer.materials[1].SetInt("_OutLine", 0); // 아웃라인 빼주고
                _isRenderOutLine = false; // 아웃라인 렌더상태를 꺼주고
                OnAimPopdown?.Invoke(); // 상호작용 UI를 꺼주고
                _canEnterInteractive = false; // 상호작용 불가능 상태로 만든다
            }
        }

        if (Input.GetKeyDown(KeyCode.V) && _canEnterInteractive == true)
        {
            hit.collider.gameObject.GetComponent<RotateObject>().Rotate(1f);
        }
    }

    private void OutlineRender()
    {
        _meshRenderer = hit.collider.gameObject.GetComponent<MeshRenderer>();
        _meshRenderer.materials[1].SetInt("_OutLine", 1);
    }
    
}
