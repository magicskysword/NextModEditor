using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public abstract class UIComBase : MonoBehaviour
{
    private Dictionary<string, GameObject> _children = new Dictionary<string, GameObject>();

    public RectTransform RectTransform => (RectTransform)transform;
    public bool IsInit { get; private set; }

    protected virtual void OnPreInit()
    {
        
    }

    protected virtual void OnInit()
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
}