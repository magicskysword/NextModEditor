using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UniRx;
using UnityEngine;

public class UIMgr : MonoBehaviour
{
    public static UIMgr Instance { get; private set; }

    public RectTransform rootBottom;
    public RectTransform rootMiddle;
    public RectTransform rootTop;
    public RectTransform rootModel;

    public GameObject modelMask;
    
    private Dictionary<string, UIPanelBase> _panels = new Dictionary<string, UIPanelBase>();
    private HashSet<UIPanelBase> _modelPanel = new HashSet<UIPanelBase>();

    private void Awake()
    {
        Instance = this;
        Observable.EveryUpdate()
            .Where(_=> Input.GetKeyDown(KeyCode.Escape) && _modelPanel != null)
            .Subscribe(_ =>
            {
                var ui = GetTopModelPanel();
                if (ui is IUIClose uiClose)
                {
                    uiClose.OnCloseUI();
                }
            });
    }
    
    public RectTransform GetLayerRoot(UILayer uiLayer)
    {
        switch (uiLayer)
        {
            case UILayer.Bottom:
                return rootBottom;
            case UILayer.Middle:
                return rootMiddle;
            case UILayer.Top:
                return rootTop;
            case UILayer.Model:
                return rootModel;
            default:
                throw new ArgumentOutOfRangeException(nameof(uiLayer), uiLayer, null);
        }
    }

    private static string GetStaticValue<T>(string staticField)
    {
        var panelName = typeof(T)
            .GetProperty(staticField, BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy)
            ?.GetValue(null)
            .ToString();
        return panelName;
    }
    
    public T GetPanel<T>(UILayer layer) where T : UIPanelBase
    {
        var panelName = GetStaticValue<T>("PanelName");
        
        if(string.IsNullOrEmpty(panelName))
            return null;

        if (_panels.ContainsKey(panelName))
            return _panels[panelName] as T;

        var go = Instantiate(Resources.Load<GameObject>($"UI/{panelName}"), 
            GetLayerRoot(layer), 
            false);
        
        var panel = go.AddComponent<T>();

        _panels.Add(panelName,panel);
        panel.Layer = layer;
        panel.Init();
        
        return panel;
    }

    public void DestroyPanel<T>() where T : UIPanelBase
    {
        var panelName = GetStaticValue<T>("PanelName");
        
        if(string.IsNullOrEmpty(panelName))
            return;

        if (!_panels.ContainsKey(panelName))
            return;
        
        var panel = _panels[panelName];
        
        _panels.Remove(panelName);
        panel.Hide();
        panel.DestroySelf();
    }

    public void PushModelPanel<T>(T panel) where T : UIPanelBase
    {
        _modelPanel.Add(panel);
        panel.Canvas.sortingOrder = 100 + _modelPanel.Count * 2 + 1;
        modelMask.SetActive(true);
    }
    
    public void PopModelPanel<T>(T panel) where T : UIPanelBase
    {
        _modelPanel.Remove(panel);
        if (_modelPanel.Count == 0)
            modelMask.SetActive(false);
        else
            modelMask.GetComponent<Canvas>().sortingOrder = _modelPanel.Max(ui => ui.Canvas.sortingOrder) - 1;
    }

    public UIPanelBase GetTopModelPanel()
    {
        if(_modelPanel.Count == 0)
            return null;
        return _modelPanel.OrderByDescending(x=>x.Canvas.sortingOrder).First();
    }
    
    public T CreateCom<T>(Transform parent = null) where T : UIComBase
    {
        var comName = GetStaticValue<T>("ComName");
        
        if(string.IsNullOrEmpty(comName))
            return null;

        var go = Instantiate(Resources.Load<GameObject>($"UICom/{comName}"), 
            parent, 
            false);
        
        var com = go.AddComponent<T>();
        
        com.Init();
        
        return com;
    }
}