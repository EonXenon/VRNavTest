using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MonitorHandler : MonoBehaviour
{
    InputLayer inputLayer;
    InputMaster config;

    [SerializeField]
    Text functionText;
    [SerializeField]
    Text itemText;

    [SerializeField]
    PlayerController player;

    [SerializeField]
    CourseController[] courses;

    enum VerticalMenu
    {
        Translation,
        Rotation,
        Courses
    }

    int verticalIndex = 0;
    int verticalIndexLimit = 2;
    int horizontalIndex = 0;
    int horizontalIndexLimit = 0;

    // Start is called before the first frame update
    void Start()
    {
        player.HookMonitor(this);
    }

    public void HookInput(InputLayer layer)
    {
        inputLayer = layer;
    }

    public void HookConfig(InputMaster master)
    {
        config = master;
        if (config == null) return;

        UpdateVertical((VerticalMenu)verticalIndex);

        config.Monitor.MenuUp.started += MenuUp;
        config.Monitor.MenuDown.started += MenuDown;
        config.Monitor.MenuLeft.started += MenuLeft;
        config.Monitor.MenuRight.started += MenuRight;
        config.Monitor.MenuSelect.started += MenuSelect;

    }

    void MenuUp(InputAction.CallbackContext context)
    {
        if (--verticalIndex < 0) verticalIndex = verticalIndexLimit;
        UpdateVertical((VerticalMenu)verticalIndex);
    }

    void MenuDown(InputAction.CallbackContext context)
    {
        if (++verticalIndex > verticalIndexLimit) verticalIndex = 0;
        UpdateVertical((VerticalMenu)verticalIndex);
    }

    void MenuLeft(InputAction.CallbackContext context)
    {
        if (--horizontalIndex < 0) horizontalIndex = horizontalIndexLimit;
        UpdateHorizontal((VerticalMenu)verticalIndex);
    }

    void MenuRight(InputAction.CallbackContext context)
    {
        if (++horizontalIndex > horizontalIndexLimit) horizontalIndex = 0;
        UpdateHorizontal((VerticalMenu)verticalIndex);
    }

    void MenuSelect(InputAction.CallbackContext context)
    {
        if((VerticalMenu)verticalIndex == VerticalMenu.Courses)
            courses[horizontalIndex].gameObject.SetActive(!courses[horizontalIndex].gameObject.activeSelf);
    }

    // Update is called once per frame
    void Update()
    {
        if (config != null) enabled = false;
        else inputLayer.HookMonitor(this);
    }

    void UpdateVertical(VerticalMenu t)
    {
        switch (t)
        {
            case VerticalMenu.Translation:
                {
                    horizontalIndex = (int)inputLayer.translationType;
                    horizontalIndexLimit = (int)InputLayer.TranslationType.DevMode;
                    functionText.text = "Translation:";
                    itemText.text = inputLayer.translationType.ToString();
                    return;
                }
            case VerticalMenu.Rotation:
                {
                    horizontalIndex = (int)inputLayer.rotationType;
                    horizontalIndexLimit = (int)InputLayer.RotationType.DevMode;
                    functionText.text = "Rotation:";
                    itemText.text = inputLayer.rotationType.ToString();
                    return;
                }
            case VerticalMenu.Courses:
                {
                    horizontalIndex = 0;
                    horizontalIndexLimit = courses.Length - 1;
                    functionText.text = "Course:";
                    itemText.text = courses[horizontalIndex].name;
                    return;
                }
        }

    }

    void UpdateHorizontal(VerticalMenu t)
    {
        switch (t)
        {
            case VerticalMenu.Translation:
                {
                    inputLayer.translationType = (InputLayer.TranslationType)horizontalIndex;
                    itemText.text = inputLayer.translationType.ToString();
                    return;
                }
            case VerticalMenu.Rotation:
                {
                    inputLayer.rotationType = (InputLayer.RotationType)horizontalIndex;
                    itemText.text = inputLayer.rotationType.ToString();
                    return;
                }
            case VerticalMenu.Courses:
                {
                    itemText.text = courses[horizontalIndex].name;
                    return;
                }
        }

    }
}
