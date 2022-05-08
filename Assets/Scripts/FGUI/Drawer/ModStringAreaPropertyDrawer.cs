using System;
using FairyGUI;
using UIPkg.Main;

public class ModStringAreaPropertyDrawer : ModPropertyDrawer
{
    private string _drawerName;
    private UIComStringAreaDrawer Drawer => (UIComStringAreaDrawer)Component;
    private Action<string> _setter;
    private Func<string> _getter;

    public ModStringAreaPropertyDrawer(string drawerName,Action<string> setter,Func<string> getter)
    {
        _drawerName = drawerName;
        _setter = setter;
        _getter = getter;
    }
    
    protected override GComponent OnCreateCom()
    {
        var drawer = UIComStringAreaDrawer.CreateInstance();
        drawer.m_inContent.onFocusOut.Add(() => OnSetProperty(drawer.m_inContent.text));
        drawer.title = _drawerName;
        drawer.m_btnEdit.onClick.Set(()=>
        {
            WindowStringInputDialog.CreateDialog(
                "main.dialog.textEdit".I18N(),
                OnGetProperty(), 
                OnConfirmEdit);
        });
        return drawer;
    }

    private void OnConfirmEdit(string str)
    {
        OnSetProperty(str);
        Refresh();
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