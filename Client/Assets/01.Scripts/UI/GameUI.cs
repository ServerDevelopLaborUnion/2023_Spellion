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
}
