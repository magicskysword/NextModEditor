using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public partial class UIComListDrawer
{
    public delegate void ListItemChangeOrder(int from, int to);

    public Action OnBtnAddClick;
    public Action<IListDrawerItem> OnBtnRemoveClick;
    public Action<IListDrawerItem> OnBtnEditClick;
    public ListItemChangeOrder OnListItemChangeOrder;
    public List<IListDrawerItem> Items { get; set; } = new List<IListDrawerItem>();

    public bool CanDrag
    {
        get => rolstMain.IsDraggable;
        set => rolstMain.IsDraggable = value;
    }

    public string Title
    {
        get => txtTitle.text;
        set => txtTitle.text = value;
    }
    
    protected override void OnInit()
    {
        btnAdd.OnClickAsObservable()
            .Subscribe(_ => OnBtnAddClick?.Invoke());
        rolstMain.OnElementGrabbed.AsObservable()
            .Subscribe(eStruct =>
            {
                var element = eStruct.DroppedObject.GetComponent<ReorderableListElement>();
                element.IsTransferable = false;
                var canvas = eStruct.DroppedObject.AddComponent<Canvas>();
                canvas.overrideSorting = true;
                canvas.sortingOrder = 30000;
            });
        rolstMain.OnElementDropped.AsObservable()
            .Subscribe(eStruct =>
            {
                OnListItemChangeOrder?.Invoke(eStruct.FromIndex, eStruct.ToIndex);
            });
        rolstMain.OnElementAdded.AsObservable()
            .Subscribe(eStruct =>
            {
                var canvas = eStruct.DroppedObject.GetComponent<Canvas>();
                if(canvas != null)
                    Destroy(canvas);
            });
    }

    public void Clear()
    {
        rolstMain.transform.Clear();
        Items.Clear();
    }

    public T AddItem<T>() where T : UIComBase, IListDrawerItem
    {
        var item = UIMgr.Instance.CreateCom<T>(rolstMain.transform);
        item.BtnRemove.OnClickAsObservable().Subscribe(_ => OnBtnRemoveClick?.Invoke(item));
        item.BtnEdit.OnClickAsObservable().Subscribe(_ => OnBtnEditClick?.Invoke(item));
        Items.Add(item);
        return item;
    }

    public void RemoveItem<T>(T item) where T : UIComBase, IListDrawerItem
    {
        Items.Remove(item);
        Destroy(item.gameObject);
    }
}

public interface IListDrawerItem
{
    public Button BtnRemove { get; }
    public Button BtnEdit { get; }
}