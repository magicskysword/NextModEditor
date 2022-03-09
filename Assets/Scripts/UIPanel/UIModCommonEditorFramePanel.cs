using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract partial class UIModCommonEditorFramePanel : IModDataEditor
{
    private ModProject _bindProject;
    private IModData _selectedItem;

    public ModProject BindProject
    {
        get => _bindProject;
        set
        {
            _bindProject = value;
            RefreshUI();
        }
    }

    public IModData SelectModData
    {
        get => _selectedItem;
        set
        {
            _selectedItem = value;
            CommonEditor.RefreshItem(_selectedItem);
        }
    }

    public UIComModCommonEditor CommonEditor { get;private set; }
    public ModDataListSource VirtualDataListSource { get; set; }
    
    public abstract IList DataList { get; }

    #region Drawer

    #endregion

    protected sealed override void OnInit()
    {
        CommonEditor = UIMgr.Instance.CreateCom<UIComModCommonEditor>(transform);

        // 刷新所有Drawer
        CommonEditor.OnRefreshItem = OnEditorRefresh;

        CommonEditor.OnClickRemoveItem = OnRemoveItem;

        CommonEditor.OnClickAddItem = OnAddItem;

        CommonEditor.OnFilterChange = RefreshUI;

        VirtualDataListSource = new ModDataListSource();
        VirtualDataListSource.RendererItem += (item, data) => item.txtTab.text = $"{GetItemName(data)}";
        VirtualDataListSource.SelectData += item => SelectModData = item;

        OnInitEditor();
    }

    protected virtual void OnAddItem()
    {
        var maxIndex = BindProject.CreateAvatarData.Count > 0
            ? BindProject.CreateAvatarData.Max(item => item.ID)
            : 0;
        var newData = new ModCreateAvatarData()
        {
            ID = maxIndex + 1
        };
        DataList.Add(newData);
        SelectModData = newData;
    }

    protected virtual void OnRemoveItem()
    {
        if (SelectModData != null)
        {
            UIConfirmBoxPanel.ShowMessage(
                "警告",
                $"即将删除 {GetItemName(SelectModData)} ，该操作不可恢复，是否继续？",
                onOk: () =>
                {
                    DataList.Remove(SelectModData);
                    RefreshUI();
                });
        }
    }

    protected abstract void OnInitEditor();

    protected abstract void OnEditorRefresh(IModData data);

    protected abstract bool OnFilterData(IModData data, string filter);

    protected abstract string GetItemName(IModData data);

    private void RefreshUI()
    {
        var filter = CommonEditor.Filter;
        var filterList = DataList.Cast<IModData>().Where(data=>OnFilterData(data,filter)).ToList();
        VirtualDataListSource.DataList = filterList;
        VirtualDataListSource.SelectedIndex = 0;
        CommonEditor.SetItemList(VirtualDataListSource);
        if (filterList.Count > 0)
        {
            SelectModData = filterList[0];
        }
        else
        {
            SelectModData = null;
        }
    }
}