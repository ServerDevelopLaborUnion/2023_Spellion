using System;
using System.Collections;
using System.Collections.Generic;
using Packet;
using UnityEngine;
using UnityEngine.UIElements;

public class GameUI : MonoBehaviour
{
    private UIDocument _uiDocument;

    private VisualElement _root;

    private void Awake()
    {
        _uiDocument = GetComponent<UIDocument>();
    }

    private void OnEnable()
    {
        _root = _uiDocument.rootVisualElement;

        Button connectBtn = _root.Q<Button>("connectBtn");
        Button disconnectBtn = _root.Q<Button>("disconnectBtn");
        Button sendTestBtn = _root.Q<Button>("sendTestBtn");

        VisualElement hpSliderValue = _root.Q<VisualElement>("hpSlider").Q<VisualElement>("value");
        hpSliderValue.style.width = new StyleLength(Length.Percent(50f));

        connectBtn.RegisterCallback<ClickEvent>(e => {
            SocketManager.Instance.Connection();
        });

        disconnectBtn.RegisterCallback<ClickEvent>(e => {
            SocketManager.Instance.Disconnect();
        });

        sendTestBtn.RegisterCallback<ClickEvent>(e => {
            MsgBox box = new MsgBox{Context = "Test"};
            SocketManager.Instance.RegisterSend(MSGID.Msgbox, box);
        });
    }

    public void SetPause(bool value)
    {
        VisualElement container = _root.Q("Container");
        if(!container.ClassListContains("pause") && value) container.AddToClassList("pause");
        else if(container.ClassListContains("pause") && !value) container.RemoveFromClassList("pause");
    }
}
