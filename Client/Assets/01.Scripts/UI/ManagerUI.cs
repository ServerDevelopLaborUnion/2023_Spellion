using System;
using System.Collections;
using System.Collections.Generic;
using Packet;
using UnityEngine;
using UnityEngine.UIElements;

public class ManagerUI : MonoBehaviour
{
    private UIDocument _uiDoc;
    private VisualElement _getName;
    private TextField _nameField;
    private Button _confirmBtn;
    private ProgressBar _loadingBar;

    private void Awake()
    {
        _uiDoc = GetComponent<UIDocument>();
    }

    private void OnEnable()
    {
        VisualElement root = _uiDoc.rootVisualElement;
        _getName = root.Q("GetName");

        _nameField = _getName.Q<TextField>("TextField");
        _confirmBtn = _getName.Q<Button>("ConfirmBtn");

        _loadingBar = root.Q<ProgressBar>("LoadingBar");

        _confirmBtn.RegisterCallback<ClickEvent>(ev => {
            SocketManager.Instance.RegisterSend(MSGID.CRegisterReq, new C_Register_Req{Name = _nameField.value});
        });
    }

    public void ShowLoadingBar()
    {
        _loadingBar.AddToClassList("on");
    }

    public void SetLoadingProgress(float value)
    {
        _loadingBar.value = value;
    }

    public void HideLoadingBar()
    {
        _loadingBar.RemoveFromClassList("on");
    }

    public void GetName()
    {
        _getName.AddToClassList("on");
    }
}