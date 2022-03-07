using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class UIModCreateAvatarEditorPanel : IModDataEditor
{
    private ModProject _bindProject;
    private ModCreateAvatarData _selectedItem;

    public ModProject BindProject
    {
        get => _bindProject;
        set
        {
            _bindProject = value;
            RefreshUI();
        }
    }

    public IModData SelectModData => SelectedItem;

    public ModCreateAvatarData SelectedItem
    {
        get => _selectedItem;
        set
        {
            _selectedItem = value;
            CommonEditor.RefreshItem(_selectedItem);
        }
    }

    public UIComModCommonEditor CommonEditor { get; set; }
    public ModDataListSource<ModCreateAvatarData> DataListSource { get; set; }

    #region Drawer

    public UIComInputIdDrawer IdDrawer { get; set; }
    public UIComInputTextDrawer NameDrawer { get; set; }
    public UIComInputNumberDrawer GroupDrawer { get; set; }
    public UIComInputNumberDrawer CostDrawer { get; set; }
    public UIComDropdownDrawer CreateTypeDrawer { get; set; }
    public UIComSeidListDrawer SeidListDrawer { get; set; }
    public UIComDropdownDrawer RequireLevelDrawer { get; set; }
    public UIComInputTextAreaDrawer DescDrawer { get; set; }
    public UIComInputTextAreaDrawer InfoDrawer { get; set; }

    #endregion

    protected override void OnInit()
    {
        CommonEditor = UIMgr.Instance.CreateCom<UIComModCommonEditor>(transform);
        IdDrawer = CommonEditor.AddEditorDrawer<UIComInputIdDrawer>();
        NameDrawer = CommonEditor.AddEditorDrawer<UIComInputTextDrawer>();
        GroupDrawer = CommonEditor.AddEditorDrawer<UIComInputNumberDrawer>();
        CostDrawer = CommonEditor.AddEditorDrawer<UIComInputNumberDrawer>();
        CreateTypeDrawer = CommonEditor.AddEditorDrawer<UIComDropdownDrawer>();
        SeidListDrawer = CommonEditor.AddEditorDrawer<UIComSeidListDrawer>();
        RequireLevelDrawer = CommonEditor.AddEditorDrawer<UIComDropdownDrawer>();
        DescDrawer = CommonEditor.AddEditorDrawer<UIComInputTextAreaDrawer>();
        InfoDrawer = CommonEditor.AddEditorDrawer<UIComInputTextAreaDrawer>();
        
        IdDrawer.Title = "ID";
        IdDrawer.BindEditor(this,
            ()=>BindProject.CreateAvatarData,
            data =>
            {
                var curData = (ModCreateAvatarData)data;
                return $"{curData.ID} {curData.Name}";
            },
            (data,newId) =>
            {
                BindProject.CreateAvatarSeidDataGroup.ChangeSeidID(data.ID, newId);
                data.ID = newId;
                BindProject.CreateAvatarData.ModSort();
                CommonEditor.RefreshItem(data);
            },
            (data,otherData) =>
            {
                BindProject.CreateAvatarSeidDataGroup.SwiftSeidID(otherData.ID, data.ID);
                (otherData.ID, data.ID) = (data.ID, otherData.ID);
                BindProject.CreateAvatarData.ModSort();
                CommonEditor.RefreshItem(data);
            },
            () =>
            {
                IdDrawer.Content = SelectedItem.ID.ToString();
            });

        NameDrawer.Title = "名称";
        NameDrawer.EndEdit = str =>
        {
            SelectedItem.Name = str;
            CommonEditor.RefreshItem(SelectedItem);
        };
        
        GroupDrawer.Title = "分组";
        GroupDrawer.EndEdit = num => SelectedItem.Group = num;
        
        CostDrawer.Title = "消耗";
        CostDrawer.EndEdit = num => SelectedItem.Cost = num;

        CreateTypeDrawer.Title = "分类";
        CreateTypeDrawer.SetOptions(
            ModMgr.Instance.CreateAvatarDataTalentTypes
                .Select(type => $"{type.TypeID} {type.TypeName}")
                .ToList());
        CreateTypeDrawer.ValueChange = index =>
        {
            var talentType = ModMgr.Instance.CreateAvatarDataTalentTypes[index];
            SelectedItem.SetTalentType(talentType);
        };
        
        SeidListDrawer.Title = "功能";
        SeidListDrawer.BindSeid(this,
            () => ModMgr.Instance.CreateAvatarSeidMetas,
            () => BindProject.CreateAvatarSeidDataGroup,
            () => SelectedItem.SeidList);
        
        RequireLevelDrawer.Title = "解锁需求";
        RequireLevelDrawer.SetOptions(ModMgr.Instance.CreateAvatarDataLevelTypes
            .Select(type => $"{type.TypeID} {type.TypeName}")
            .ToList());
        RequireLevelDrawer.ValueChange = index =>
        {
            var talentType = ModMgr.Instance.CreateAvatarDataLevelTypes[index];
            SelectedItem.RequireLevel = talentType.TypeID;
        };
        
        DescDrawer.Title = "效果";
        DescDrawer.EndEdit += str => SelectedItem.Desc = str;
        
        InfoDrawer.Title = "描述";
        InfoDrawer.EndEdit += str => SelectedItem.Info = str;
        
        // 刷新所有Drawer
        CommonEditor.OnRefreshItem = data =>
        {
            var curData = (ModCreateAvatarData)data;
            IdDrawer.Content = curData.ID.ToString();
            NameDrawer.Content = curData.Name;
            GroupDrawer.Content = curData.Group.ToString();
            CostDrawer.Content = curData.Cost.ToString();
            DescDrawer.Content = curData.Desc;
            InfoDrawer.Content = curData.Info;
            CreateTypeDrawer.Select(ModMgr.Instance.CreateAvatarDataTalentTypes
                .TryFind(type => type.TypeID == curData.CreateTypeRelation));
            RequireLevelDrawer.Select(ModMgr.Instance.CreateAvatarDataLevelTypes
                .TryFind(type => type.TypeID == curData.RequireLevel));
            
            SeidListDrawer.Refresh(meta=>$"{meta.ID} {meta.Name}");

            var dataIndex = BindProject.CreateAvatarData.FindIndex(searchData => searchData == data);
            CommonEditor.ItemListScrollTo(dataIndex);
        };

        CommonEditor.OnClickRemoveItem = () =>
        {
            if (SelectedItem != null)
            {
                UIConfirmBoxPanel.ShowMessage(
                    "警告",
                    $"即将删除 {SelectedItem.ID} {SelectedItem.Name} ，该操作不可恢复，是否继续？",
                    onOk: () =>
                    {
                        BindProject.CreateAvatarData.Remove(SelectedItem);
                        RefreshUI();
                    });
            }
        };
        
        CommonEditor.OnClickAddItem = () =>
        {
            var maxIndex = BindProject.CreateAvatarData.Count > 0 
                ? BindProject.CreateAvatarData.Max(item => item.ID)
                : 0;
            var newData = new ModCreateAvatarData()
            {
                ID = maxIndex + 1
            };
            BindProject.CreateAvatarData.Add(newData);
            SelectedItem = newData;
        };

        DataListSource = new ModDataListSource<ModCreateAvatarData>();
        DataListSource.RendererItem += (item,data) => item.txtTab.text = $"{data.ID} {data.Name}";;
        DataListSource.SelectData += item => SelectedItem = item;
    }

    private void RefreshUI()
    {
        DataListSource.DataList = BindProject.CreateAvatarData;
        DataListSource.SelectedIndex = 0;
        CommonEditor.SetItemList(DataListSource);
        if (BindProject.CreateAvatarData.Count > 0)
        {
            SelectedItem = BindProject.CreateAvatarData[0];
        }
        else
        {
            SelectedItem = null;
        }
    }
}