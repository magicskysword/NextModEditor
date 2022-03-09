using System;
using System.Linq;
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
    public Action OnFilterChange;
    public ModDataListSource ListSource { get; set; }

    public string Filter => inMain.text;

    protected override void OnInit()
    {
        btnItemAdd.OnClickAsObservable().Subscribe(_ => OnClickAddItem?.Invoke());
        btnItemRemove.OnClickAsObservable().Subscribe(_ => OnClickRemoveItem?.Invoke());
        inMain.OnValueChangedAsObservable().Subscribe(_ =>
        {
            OnFilterChange?.Invoke();
        });
    }

    public void SetItemList(ModDataListSource listSource)
    {
        ListSource = listSource;
        vlstItems.SetSource(ListSource);
        lstTabs.verticalNormalizedPosition = 1f;
        vlstItems.Invalidate();
    }

    public void ItemListScrollTo(int index)
    {
        var scrollRect = vlstItems.GetCenteredScrollPosition(index);
        ListSource.SelectedIndex = index;
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
            var dataIndex = ListSource.DataList.FindIndex(searchData => searchData == data);
            ItemListScrollTo(dataIndex);
            Observable.EveryUpdate().First().Subscribe(_ =>
            {
                goDrawerRoot.ForceRebuildLayoutImmediate();
            });
        }
    }
}