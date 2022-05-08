using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public static class UIUtils
{
    public static void ForceRebuildLayoutImmediate(this RectTransform rectTransform)
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
    }
    
    public static void MarkLayoutForRebuild(this RectTransform rectTransform)
    {
        LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
    }

    public static void Clear(this Transform transform)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Object.Destroy(transform.GetChild(i).gameObject);
        }
    }
    
    public static RectTransform GetRectTransform(this Component component)
    {
        return component.transform as RectTransform;
    }
    
    public static IObservable<string> OnEndEditAsObservable(this TMP_InputField inputField)
    {
        return inputField.onEndEdit.AsObservable();
    }

    public static IObservable<string> OnValueChangedAsObservable(this TMP_InputField inputField)
    {
        return Observable.CreateWithState<string, TMP_InputField>(inputField, (i, observer) =>
        {
            observer.OnNext(i.text);
            return i.onValueChanged.AsObservable().Subscribe(observer);
        });
    }
    
    public static IObservable<int> OnValueChangedAsObservable(this TMP_Dropdown dropdown)
    {
        return Observable.CreateWithState<int, TMP_Dropdown>(dropdown, (d, observer) =>
        {
            observer.OnNext(d.value);
            return d.onValueChanged.AsObservable().Subscribe(observer);
        });
    }
}