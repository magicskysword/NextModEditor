using System;
using FairyGUI;
using UIPkg.Main;

public class ModStringPropertyDrawer : ModPropertyDrawer
{
    private string _drawerName;
    private UIComStringDrawer Drawer => (UIComStringDrawer)Component;
    private Action<string> _setter;
    private Func<string> _getter;

    public ModStringPropertyDrawer(string drawerName,Action<string> setter,Func<string> getter)
    {
        _drawerName = drawerName;
        _setter = setter;
        _getter = getter;
    }
    
    protected override GComponent OnCreateCom()
    {
        var drawer = UIComStringDrawer.CreateInstance();
        drawer.m_inContent.onFocusOut.Add(() => OnSetProperty(drawer.m_inContent.text));
        drawer.title = _drawerName;
        return drawer;
    }

    protected override void OnRefresh()
    {
        Drawer.m_inContent.text = OnGetProperty();
    }
    
    private void OnSetProperty(string text)
    {
        _setter.Invoke(text);
        OnChanged?.Invoke();
    }

    private string OnGetProperty()
    {
        return _getter.Invoke();
    }
}