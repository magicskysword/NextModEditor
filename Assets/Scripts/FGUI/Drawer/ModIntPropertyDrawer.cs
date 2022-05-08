﻿using System;
using FairyGUI;
using UIPkg.Main;

public class ModIntPropertyDrawer : ModPropertyDrawer
{
    private string _drawerName;
    private UIComIntDrawer Drawer => (UIComIntDrawer)Component;
    private Action<int> _setter;
    private Func<int> _getter;

    public ModIntPropertyDrawer(string drawerName,Action<int> setter,Func<int> getter)
    {
        _drawerName = drawerName;
        _setter = setter;
        _getter = getter;
    }
    
    protected override GComponent OnCreateCom()
    {
        var drawer = UIComIntDrawer.CreateInstance();
        drawer.BindEndEdit(OnSetProperty);
        drawer.title = _drawerName;
        return drawer;
    }

    protected override void OnRefresh()
    {
        Drawer.m_inContent.text = OnGetProperty().ToString();
    }
    
    private void OnSetProperty(int text)
    {
        _setter.Invoke(text);
        OnChanged?.Invoke();
    }

    private int OnGetProperty()
    {
        return _getter.Invoke();
    }
}