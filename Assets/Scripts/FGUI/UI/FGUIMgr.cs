using System;
using System.Collections.Generic;
using System.Reflection;
using FairyGUI;
using UIPkg.Main;
using UnityEngine;

public class FGUIMgr
{
    public static FGUIMgr I { get; } = new FGUIMgr();

    private Dictionary<string, GComponent> panelDic = new Dictionary<string, GComponent>();

    private FGUIMgr()
    {
        
    }

    public void Init()
    {
        UIConfig.defaultFont = "Alibaba-PuHuiTi-Medium";
        UIConfig.horizontalScrollBar = "ui://Main/ScrollBarH";
        UIConfig.verticalScrollBar = "ui://Main/ScrollBarV";
        UIConfig.popupMenu = "ui://Main/PopupMenu";

        MainBinder.BindAll();
        UIPackage.AddPackage("UIRes/Main");
        
        RegisterCursor("resizeH","Cursor/cursor_resize1");
        RegisterCursor("resizeV","Cursor/cursor_resize2");
    }

    private void RegisterCursor(string name,string path)
    {
        var texture2D = Resources.Load<Texture2D>(path);
        
        Stage.inst.RegisterCursor(name, texture2D, new Vector2(texture2D.width/2f,texture2D.height/2f));
    }

    public T ShowPanel<T>() where T : GComponent
    {
        T panel = (T)typeof(T)
            .GetMethod("CreateInstance", BindingFlags.Static | BindingFlags.Public)
            !.Invoke(null, Array.Empty<object>());
        
        GRoot.inst.AddChild(panel);
        panel.MakeFullScreen();
        panel.AddRelation(GRoot.inst,RelationType.Size);
        panelDic.Add(GetPanelID<T>(), panel);
        
        if(panel is IFGUIInit fguiInit)
            fguiInit.OnInit();
        
        return panel;
    }

    public string GetPanelID<T>()
    {
        return $"{typeof(T).Namespace}.{typeof(T).Name}";
    }
}