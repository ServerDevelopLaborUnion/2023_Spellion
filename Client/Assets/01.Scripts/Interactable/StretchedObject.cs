using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StretchedObject : MonoBehaviour
{
    [SerializeField]
    private GameObject _stretchedObj;
    private Transform _objTransform;

    [SerializeField]
    private float _targetScaleY;
    [SerializeField]
    private float _increaseValue;

    private float _originScaleY;

    private void Awake()
    {
        _objTransform = _stretchedObj.transform;
        _originScaleY = transform.localScale.y;
    }

    private void Update()
    {
        if (_objTransform.localScale.y < _targetScaleY)
        {
            Stretched();
        }
        else if(_objTransform.localScale.y > _targetScaleY) //조건문 바꿔야 함
        {
            Shrinking();
        }
    }

    private void Stretched()
    {
        _objTransform.localScale += new Vector3(0, _increaseValue, 0);
        _objTransform.localPosition += new Vector3(0, _increaseValue / 2, 0);
    }

    private void Shrinking()
    {
        //줄어드는 거
    }
}
