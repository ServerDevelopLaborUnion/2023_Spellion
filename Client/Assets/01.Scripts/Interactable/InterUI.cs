using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InterUI : MonoBehaviour
{
    private UIDocument _uiDocument;
    private VisualElement _interContainer;
    private Label _interLabelL;
    private Label _interLabelR;

    private void Awake()
    {
        _uiDocument = GetComponent<UIDocument>();
        VisualElement root = _uiDocument.rootVisualElement;
        _interContainer = root.Q<VisualElement>("InterContainer");
        _interLabelL = _interContainer.Q<Label>("InterLabelL");
        _interLabelR = _interContainer.Q<Label>("InterLabelR");
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
        yield return new WaitForSeconds(0.5f);
        _interLabelL.AddToClassList("onL");
        _interLabelR.AddToClassList("onR");
    }
    private IEnumerator RemoveOnClassCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        _interLabelL.RemoveFromClassList("onL");
        _interLabelR.RemoveFromClassList("onR");
    }
}
