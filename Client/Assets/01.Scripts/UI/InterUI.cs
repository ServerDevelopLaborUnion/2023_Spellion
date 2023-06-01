using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InterUI : MonoBehaviour
{
    private UIDocument _uiDocument;
    private VisualElement _interContainer;

    private void Awake()
    {
        _uiDocument = GetComponent<UIDocument>();
        VisualElement root = _uiDocument.rootVisualElement;
        _interContainer = root.Q<VisualElement>("InterContainer");
    }

    public void AddOnClass()
    {
        StartCoroutine(AddOnClassCoroutine());
    }
    
    public void RemoveOnClass()
    {
        StartCoroutine(RemoveOnClassCoroutine());
    }
    private IEnumerator AddOnClassCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        _interContainer.AddToClassList("on");
    }
    private IEnumerator RemoveOnClassCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        _interContainer.RemoveFromClassList("on");
    }
}
