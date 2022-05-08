using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public abstract class UIPanelBase : MonoBehaviour
{
    private Dictionary<string, GameObject> _children = new Dictionary<string, GameObject>();

    public RectTransform RectTransform => (RectTransform)transform;
    public Canvas Canvas => GetComponent<Canvas>();
    public UILayer Layer { get; set; }
    public bool IsShow { get; private set; }
    public bool IsInit { get; private set; }

    protected virtual void OnPreInit()
    {
        
    }

    protected virtual void OnInit()
    {
        
    }

    protected virtual void OnShow()
    {
        
    }

    protected virtual void DoShowAnimation()
    {
        ShowComplete();
    }

    protected virtual void OnHide()
    {
        
    }

    protected virtual void DoHideAnimation()
    {
        HideComplete();
    }

    protected virtual void OnPanelDestroy()
    {
        
    }

    public void Init()
    {
        if(IsInit)
            return;

        IsInit = true;
        
        var transforms = gameObject.GetComponentsInChildren<Transform>(true);
        foreach (var go in transforms.Select(t=>t.gameObject))
        {
            if(go.name.StartsWith("g:",StringComparison.OrdinalIgnoreCase))
                _children.Add(go.name,go);
        }
        
        Canvas.overrideSorting = true;
        switch (Layer)
        {
            case UILayer.Bottom:
                Canvas.sortingOrder = 0;
                break;
            case UILayer.Middle:
                Canvas.sortingOrder = 20;
                break;
            case UILayer.Top:
                Canvas.sortingOrder = 50;
                break;
            case UILayer.Model:
                Canvas.sortingOrder = 100;
                break;
        }
        
        OnPreInit();
        OnInit();
    }

    protected T FindBindComponent<T>(string goName) where T : Component
    {
        if (!_children.TryGetValue(goName, out var go))
        {
            return null;
        }

        return go.GetComponent<T>();
    }

    public void Show()
    {
        if(IsShow)
            return;
        gameObject.SetActive(true);
        IsShow = true;
        if(Layer == UILayer.Model)
            UIMgr.Instance.PushModelPanel(this);
        DoShowAnimation();
    }

    protected void ShowComplete()
    {
        OnShow();
    }

    public void Hide()
    {
        if(!IsShow)
            return;
        IsShow = false;
        if(Layer == UILayer.Model)
            UIMgr.Instance.PopModelPanel(this);
        DoHideAnimation();
    }
    
    protected void HideComplete()
    {
        OnHide();
        gameObject.SetActive(false);
    }

    public void DestroySelf()
    {
        OnPanelDestroy();
        Destroy(gameObject);
    }
}