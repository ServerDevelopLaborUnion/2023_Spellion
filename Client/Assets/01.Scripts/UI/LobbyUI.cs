using System;
using System.Collections;
using System.Collections.Generic;
using Google.Protobuf.Collections;
using Packet;
using UnityEngine;
using UnityEngine.UIElements;

public class LobbyUI : MonoBehaviour
{
    private UIDocument _uiDoc;

    private VisualElement _currentContainer;
    private VisualElement _currentTopBar;

    private VisualElement _mainTopBar;
    private Button _play, _weapons, _social, _store;
    private VisualElement _image;
    private Label _name, _level;
    private Slider _levelBar;

    private VisualElement _roomTopBar;
    private Label _title, _sub;
    private Button _readyBtn, _exitBtn, _gamePlayBtn;
    
    private VisualElement _mainContainer;
    
    private VisualElement _selectModeContainer;
    private Button _allKill, _payload, _capture, _bomb;

    private VisualElement _roomContainer;

    [SerializeField]
    private VisualTreeAsset _player;

    private void Awake()
    {
        _uiDoc = GetComponent<UIDocument>();
    }

    private void OnEnable()
    {
        VisualElement root = _uiDoc.rootVisualElement;
        
        MainTopBarInit(root);
        RoomTopBarInit(root);

        _mainContainer = root.Q("MainContainer");

        SelectModeInit(root);
        RoomContainerInit(root);

        _currentContainer = _mainContainer;
        _currentTopBar = _mainTopBar;
    }

    private void Start()
    {
        if(GameManager.Instance.MyData != null)
            SetUserData(GameManager.Instance.MyData);
    }

    public void InitRoom()
    {
        VisualElement mySlot = _roomContainer.Q($"0");
        mySlot.RemoveFromClassList("empty");

        VisualElement player = mySlot.Q("Player");
        player.Q<Label>("Name").text = GameManager.Instance.MyData.Name;
        player.Q<Label>("Name").style.color = new StyleColor(Color.yellow);
        player.Q<Label>("Level").text = GameManager.Instance.MyData.Level.ToString();

        SetCurrentTopBar(_roomTopBar);
        SetCurrentContainer(_roomContainer);
    }
    public void InitRoom(int index, IList<RoomMember> members)
    {
        VisualElement mySlot = _roomContainer.Q($"{index}");
        mySlot.RemoveFromClassList("empty");

        VisualElement player = mySlot.Q("Player");
        player.Q<Label>("Name").text = GameManager.Instance.MyData.Name; 
        player.Q<Label>("Name").style.color = new StyleColor(Color.yellow);
        player.Q<Label>("Level").text = GameManager.Instance.MyData.Level.ToString();

        foreach(RoomMember member in members)
        {
            VisualElement slot = _roomContainer.Q($"{member.Index}");
            slot.RemoveFromClassList("empty");

            player = slot.Q("Player");
            player.Q<Label>("Name").text = member.User.Name;
            player.Q<Label>("Name").style.color = new StyleColor(Color.white);
            player.Q<Label>("Level").text = member.User.Level.ToString();
        }

        SetCurrentTopBar(_roomTopBar);
        SetCurrentContainer(_roomContainer);
    }

    public void AddMember(RoomMember member)
    {
        VisualElement slot = _roomContainer.Q($"{member.Index}");
        slot.RemoveFromClassList("empty");

        VisualElement player = slot.Q("Player");
        player.Q<Label>("Name").text = member.User.Name;
        player.Q<Label>("Name").style.color = new StyleColor(Color.white);
        player.Q<Label>("Level").text = member.User.Level.ToString();

        SetCurrentTopBar(_roomTopBar);
        SetCurrentContainer(_roomContainer);
    }

    public void JoinRoom()
    {
        // SetCurrentTopBar(_roomTopBar);
        // SetCurrentContainer(_roomContainer);
        C_Join_Req req = new C_Join_Req{Mode = Packet.GameMode.AllKill};
        SocketManager.Instance.RegisterSend(MSGID.CJoinReq, req);
        // TODO: Show Waiting Popup & Set timeout
    }

    public void ExitRoom()
    {
        SocketManager.Instance.RegisterSend(MSGID.CExitRoom, new C_Exit_Room());
        SetCurrentTopBar(_mainTopBar);
        SetCurrentContainer(_mainContainer);
    }
    
    public void ClearRoom()
    {
        for(int i = 0; i < 6; i++)
        {
            VisualElement slot = _roomContainer.Q($"{i}");
            if(!slot.ClassListContains("empty"))
            {
                slot.AddToClassList("empty");
            }
        }
    }

    private void SetCurrentContainer(VisualElement container)
    {
        if(_currentContainer.ClassListContains("on"))
        {
            _currentContainer.RemoveFromClassList("on");
        }
        _currentContainer = container;
        _currentContainer.AddToClassList("on");
    }

    private void SetCurrentTopBar(VisualElement topBar)
    {
        if(_currentTopBar.ClassListContains("on"))
        {
            _currentTopBar.RemoveFromClassList("on");
        }
        _currentTopBar = topBar;
        _currentTopBar.AddToClassList("on");
    }

    private void MainTopBarInit(VisualElement root)
    {
        _mainTopBar = root.Q("MainTopBar");
        _play = _mainTopBar.Q<Button>("PlayBtn");
        _weapons = _mainTopBar.Q<Button>("WeaponBtn");
        _social = _mainTopBar.Q<Button>("SocialBtn");
        _store = _mainTopBar.Q<Button>("StoreBtn");

        _image = _mainTopBar.Q("Image");
        _name = _mainTopBar.Q<Label>("Name");
        _level = _mainTopBar.Q<Label>("Level");
        _levelBar = _mainTopBar.Q<Slider>("LevelSlider");

        _play.RegisterCallback<ClickEvent>(PlayButtonHandle);
    }

    private void RoomTopBarInit(VisualElement root)
    {
        _roomTopBar = root.Q("RoomTopBar");
    }

    private void SelectModeInit(VisualElement root)
    {
        _selectModeContainer = root.Q("SelectModeContainer");
        VisualElement modeList = _selectModeContainer.Q("ModeList");
        _allKill = modeList.Q<Button>("AllKill");
        _payload = modeList.Q<Button>("Payload");
        _capture = modeList.Q<Button>("Capture");
        _bomb    = modeList.Q<Button>("Bomb");

        _allKill.RegisterCallback<ClickEvent>(e => JoinRoom());
    }

    private void RoomContainerInit(VisualElement root)
    {
        _roomContainer = root.Q("RoomContainer");
        _exitBtn = root.Q<Button>("ExitBtn");
        _readyBtn = root.Q<Button>("ReadyBtn");
        _gamePlayBtn = root.Q<Button>("StartBtn");

        _exitBtn.RegisterCallback<ClickEvent>(e => ExitRoom());
        _readyBtn.RegisterCallback<ClickEvent>(e => SocketManager.Instance.RegisterSend(MSGID.CReady, new C_Ready()));
        _gamePlayBtn.RegisterCallback<ClickEvent>(e => SocketManager.Instance.RegisterSend(MSGID.CGameStart, new C_Game_Start()));
    }

    private void PlayButtonHandle(ClickEvent ev)
    {
        if(_currentContainer == _selectModeContainer)
        {
            _play.RemoveFromClassList("on");
            SetCurrentContainer(_mainContainer);
        }
        else
        {
            _play.AddToClassList("on");
            SetCurrentContainer(_selectModeContainer);
        }
    }

    public void SetUserData(User userData)
    {
        _level.text = userData.Level.ToString();
        _name.text = userData.Name;
    }

    public void FixMember(IList<RoomMember> fixedMembers)
    {
        ClearRoom();
        foreach(RoomMember member in fixedMembers)
        {
            VisualElement slot = _roomContainer.Q($"{member.Index}");
            slot.RemoveFromClassList("empty");

            VisualElement player = slot.Q("Player");
            player.Q<Label>("Name").text = member.User.Name;
            player.Q<Label>("Name").style.color = new StyleColor(member.User.Name == GameManager.Instance.MyData.Name ? Color.yellow : Color.white);
            player.Q<Label>("Level").text = member.User.Level.ToString();
        }
    }
}
