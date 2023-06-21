using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class MainLobbyUI : MonoBehaviour
{
    private UIDocument _uiDocument;
    
    private Button _playBtn;
    private Button _weaponBtn;
    private Button _socialBtn;
    private Button _storeBtn;

    private VisualElement _root;
    private VisualElement _btnContainer;
    
    private void Awake()
    {
        _uiDocument = GetComponent<UIDocument>();
        
        _root = _uiDocument.rootVisualElement;
        _btnContainer = _root.Q<VisualElement>("BtnContainer");

        _playBtn = _btnContainer.Q<Button>("PlayBtn");
        _weaponBtn = _btnContainer.Q<Button>("WeaponBtn");
        _socialBtn = _btnContainer.Q<Button>("SocialBtn");
        _storeBtn = _btnContainer.Q<Button>("StoreBtn");
        
        

    }

    private void OnEnable()
    {
        _playBtn.RegisterCallback<MouseOverEvent>(e =>
        {
            VisualElement arrow = _playBtn.Q<VisualElement>("Arrow");
            arrow.visible = true;
        });
        _playBtn.RegisterCallback<MouseDownEvent>(e =>
        {
            VisualElement arrow = _playBtn.Q<VisualElement>("Arrow");
            arrow.visible = false;
        });
        
        _weaponBtn.RegisterCallback<MouseOverEvent>(e =>
        {
            
        });
        _socialBtn.RegisterCallback<MouseOverEvent>(e =>
        {
            
        });
        _storeBtn.RegisterCallback<MouseOverEvent>(e =>
        {
            
        });
        
        
    }
}
