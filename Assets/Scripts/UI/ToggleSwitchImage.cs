using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class ToggleSwitchImage : MonoBehaviour
{
    private Toggle toggle;
    
    public Sprite imgIsOn;
    public Sprite imgIsOff;

    public void OnSwitchToggle(bool isOn)
    {
        var image = toggle.targetGraphic as Image;
        
        if(image == null)
            return;

        if (isOn)
            image.sprite = imgIsOn;
        else
            image.sprite = imgIsOff;
    }

    public void Awake()
    {
        toggle = GetComponent<Toggle>();
    }

    private void Update()
    {
        OnSwitchToggle(toggle.isOn);
    }
}