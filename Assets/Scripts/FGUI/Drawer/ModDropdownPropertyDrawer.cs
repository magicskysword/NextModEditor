using System;
using System.Collections.Generic;
using System.Linq;
using FairyGUI;
using UIPkg.Main;

public class ModDropdownPropertyDrawer : ModPropertyDrawer
{
    private string _drawerName;
    private Func<IEnumerable<string>> _optionsGetter;
    private readonly Action<int> _indexSetter;
    private readonly Func<int> _indexGetter;

    private UIComDropdownDrawer Drawer => (UIComDropdownDrawer)Component;

    public ModDropdownPropertyDrawer(string drawerName,Func<IEnumerable<string>> optionsGetter,
        Action<int> indexSetter,Func<int> indexGetter)
    {
        _drawerName = drawerName;
        _optionsGetter = optionsGetter;
        _indexSetter = indexSetter;
        _indexGetter = indexGetter;
    }
    
    protected override GComponent OnCreateCom()
    {
        var drawer = new UIComDropdownDrawer();
        drawer.title = _drawerName;
        drawer.m_dropdown.items = _optionsGetter.Invoke().ToArray();
        drawer.m_dropdown.selectedIndex = OnGetPropertyIndex();
        drawer.m_dropdown.onChanged.Add(OnDropdownChange);
        return drawer;
    }

    private void OnSetPropertyIndex(int value)
    {
        _indexSetter.Invoke(value);
        OnChanged?.Invoke();
    }

    private int OnGetPropertyIndex()
    {
        return _indexGetter.Invoke();
    }
    
    private void OnDropdownChange()
    {
        OnSetPropertyIndex(Drawer.m_dropdown.selectedIndex);
    }

    protected override void OnRefresh()
    {
        Drawer.m_dropdown.selectedIndex = OnGetPropertyIndex();
    }
}