using System;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VirtualList;

public partial class UIComModCommonEditor
{
    public Action OnClickAddItem;
    public Action OnClickRemoveItem;
    public Action<IModData> OnRefreshItem;
    public IListSourceSelected ListSourceSelected;

    protected override void OnInit()
    {
        btnItemAdd.OnClickAsObservable().Subscribe(_ => OnClickAddItem?.Invoke());
        btnItemRemove.OnClickAsObservable().Subscribe(_ => OnClickRemoveItem?.Invoke());
    }

    public void SetItemList<T>(ModDataListSource<T> listSource) where T : IModData
    {
        vlstItems.SetSource(listSource);
        ListSourceSelected = listSource;
        lstTabs.verticalNormalizedPosition = 1f;
        vlstItems.Invalidate();
    }

    public void ItemListScrollTo(int index)
    {
        var scrollRect = vlstItems.GetCenteredScrollPosition(index);
        ListSourceSelected.SelectedIndex = index;
        // lstTabs.content.anchoredPosition = scrollRect;
        lstTabs.content.DOAnchorPos(scrollRect, 0.3f);
        vlstItems.Invalidate();
    }

    public T AddEditorDrawer<T>() where T : UIComBase
    {
        var drawer = UIMgr.Instance.CreateCom<T>(goDrawerRoot);
        return drawer;
    }

    public void ShowEditor()
    {
        goDrawerRoot.gameObject.SetActive(true);
        btnItemRemove.interactable = true;
    }

    public void HideEditor()
    {
        goDrawerRoot.gameObject.SetActive(false);
        btnItemRemove.interactable = false;
    }

    public void RefreshItemList()
    {
        vlstItems.Invalidate();
    }
    
    public void RefreshItem(IModData data)
    {
        if (data == null)
        {
            HideEditor();
        }
        else
        {
            ShowEditor();
            OnRefreshItem?.Invoke(data);
            Observable.EveryUpdate().First().Subscribe(_ =>
            {
                goDrawerRoot.ForceRebuildLayoutImmediate();
            });
        }
    }
}