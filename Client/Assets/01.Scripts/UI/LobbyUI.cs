using System.Collections;
using System.Collections.Generic;
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
    private Button _exitBtn;
    
    private VisualElement _mainContainer;
    
    private VisualElement _selectModeContainer;
    private Button _allKill, _payload, _capture, _bomb;

    private VisualElement _roomContainer;

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

    public void InitRoom()
    {
        
    }

    public void JoinRoom()
    {
        SetCurrentTopBar(_roomTopBar);
        SetCurrentContainer(_roomContainer);
    }

    public void ExitRoom()
    {
        SetCurrentTopBar(_mainTopBar);
        SetCurrentContainer(_mainContainer);
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

        _exitBtn.RegisterCallback<ClickEvent>(e => ExitRoom());
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
}
