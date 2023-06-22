using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LobbyUI : MonoBehaviour
{
    private UIDocument _uiDoc;

    private VisualElement _currentContainer;

    private Button _play, _weapons, _social, _store;
    private VisualElement _image;
    private Label _name, _level;
    private Slider _levelBar;
    
    private VisualElement _mainContainer;
    
    private VisualElement _selectModeContainer;
    private Button _allKill, _payload, _capture, _bomb;

    private void Awake()
    {
        _uiDoc = GetComponent<UIDocument>();
    }

    private void OnEnable()
    {
        VisualElement root = _uiDoc.rootVisualElement;
        
        TopBarInit(root);

        _mainContainer = root.Q("MainContainer");

        SelectModeInit(root);

        _currentContainer = _mainContainer;
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

    private void TopBarInit(VisualElement root)
    {
        VisualElement topBar = root.Q("TopBar");
        _play = topBar.Q<Button>("PlayBtn");
        _weapons = topBar.Q<Button>("WeaponBtn");
        _social = topBar.Q<Button>("SocialBtn");
        _store = topBar.Q<Button>("StoreBtn");

        _image = topBar.Q("Image");
        _name = topBar.Q<Label>("Name");
        _level = topBar.Q<Label>("Level");
        _levelBar = topBar.Q<Slider>("LevelSlider");

        _play.RegisterCallback<ClickEvent>(PlayButtonHandle);
    }

    private void SelectModeInit(VisualElement root)
    {
        _selectModeContainer = root.Q("SelectModeContainer");
        VisualElement modeList = _selectModeContainer.Q("ModeList");
        _allKill = modeList.Q<Button>("AllKill");
        _payload = modeList.Q<Button>("Payload");
        _capture = modeList.Q<Button>("Capture");
        _bomb    = modeList.Q<Button>("Bomb");
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
