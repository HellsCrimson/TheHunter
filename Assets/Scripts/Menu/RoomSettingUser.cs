using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomSettingUser : MonoBehaviour
{
    public static RoomSettingUser Instance;
    
    [SerializeField] public Button isVisibleButton;
    [SerializeField] public Button isntVisibleButton;
    
    [SerializeField] public Button isJoinableButton;
    [SerializeField] public Button isntJoinableButton;
    
    [HideInInspector]
    public bool isVisible;
    [HideInInspector]
    public bool isJoinable;
    
    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        isVisible = false;
        isVisibleButton.enabled = true;
        isntVisibleButton.enabled = false;

        isJoinable = false;
        isJoinableButton.enabled = true;
        isntJoinableButton.enabled = false;
    }
    
    public void VisibleModeSelection()
    {
        VisibleModeChange();
    }
    
    public void JoinableModeSelection()
    {
        JoinableModeChange();
    }

    void VisibleModeChange()
    {
        if (isVisible)
        {
            isVisible = false;
            isVisibleButton.enabled = true;
            isntVisibleButton.enabled = false;
        }
        else
        {
            isVisible = true;
            isVisibleButton.enabled = false;
            isntVisibleButton.enabled = true;
        }
    }

    void JoinableModeChange()
    {
        if (isJoinable)
        {
            isJoinable = false;
            isJoinableButton.enabled = true;
            isntJoinableButton.enabled = false;
        }
        else
        {
            isJoinable = true;
            isJoinableButton.enabled = false;
            isntJoinableButton.enabled = true;
        }
    }
}