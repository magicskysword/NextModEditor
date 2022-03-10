using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public partial class UIModBuffEditorPanel : UIModCommonEditorFramePanel
{
    #region Drawer

    public UIComInputIdDrawer IdDrawer { get; set; }
    public UIComInputTextDrawer NameDrawer { get; set; }
    public UIComInputNumberDrawer SkillIconDrawer { get; set; }
    public UIComInputTextDrawer SkillEffectDrawer { get; set; }
    public UIComInputTextDrawer AffixDrawer { get; set; }
    public UIComTextWithContentDrawer AffixPreviewDrawer { get; set; }
    public UIComDropdownDrawer BuffTypeDrawer { get; set; }
    public UIComDropdownDrawer BuffTriggerTypeDrawer { get; set; }
    public UIComDropdownDrawer BuffRemoveTriggerTypeDrawer { get; set; }
    public UIComDropdownDrawer BuffOverlayTypeDrawer { get; set; }
    public UIComToggleDrawer BuffHideDrawer { get; set; }
    public UIComToggleDrawer BuffShowOnlyOneDrawer { get; set; }
    public UIComSeidListDrawer SeidListDrawer { get; set; }
    public UIComSeidListDrawer OuterSeidListDrawer { get; set; }
    public UIComInputTextAreaDrawer BuffDescDrawer { get; set; }
    
    #endregion
    public ModBuffData SelectedItem => (ModBuffData)SelectModData;
    public override IList DataList => BindProject.BuffData;
    public override Type ItemType => typeof(ModBuffData);

    protected override void OnInitEditor()
    {
        IdDrawer = CommonEditor.AddEditorDrawer<UIComInputIdDrawer>();
        NameDrawer = CommonEditor.AddEditorDrawer<UIComInputTextDrawer>();
        SkillIconDrawer = CommonEditor.AddEditorDrawer<UIComInputNumberDrawer>();
        SkillEffectDrawer = CommonEditor.AddEditorDrawer<UIComInputTextDrawer>();
        AffixDrawer = CommonEditor.AddEditorDrawer<UIComInputTextDrawer>();
        AffixPreviewDrawer = CommonEditor.AddEditorDrawer<UIComTextWithContentDrawer>();
        BuffTypeDrawer = CommonEditor.AddEditorDrawer<UIComDropdownDrawer>();
        BuffTriggerTypeDrawer = CommonEditor.AddEditorDrawer<UIComDropdownDrawer>();
        BuffRemoveTriggerTypeDrawer = CommonEditor.AddEditorDrawer<UIComDropdownDrawer>();
        BuffOverlayTypeDrawer = CommonEditor.AddEditorDrawer<UIComDropdownDrawer>();
        BuffHideDrawer = CommonEditor.AddEditorDrawer<UIComToggleDrawer>();
        BuffShowOnlyOneDrawer = CommonEditor.AddEditorDrawer<UIComToggleDrawer>();
        SeidListDrawer = CommonEditor.AddEditorDrawer<UIComSeidListDrawer>();
        OuterSeidListDrawer = CommonEditor.AddEditorDrawer<UIComSeidListDrawer>();
        BuffDescDrawer = CommonEditor.AddEditorDrawer<UIComInputTextAreaDrawer>();
        
        
        IdDrawer.Title = "ID";
        IdDrawer.BindEditor(this,
            ()=>BindProject.BuffData,
            data =>
            {
                var curData = (ModBuffData)data;
                return $"{curData.ID} {curData.Name}";
            },
            (data,newId) =>
            {
                BindProject.BuffSeidDataGroup.ChangeSeidID(data.ID, newId);
                data.ID = newId;
                BindProject.BuffData.ModSort();
                CommonEditor.RefreshItem(data);
            },
            (data,otherData) =>
            {
                BindProject.BuffSeidDataGroup.SwiftSeidID(otherData.ID, data.ID);
                (otherData.ID, data.ID) = (data.ID, otherData.ID);
                BindProject.BuffData.ModSort();
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
        
        SkillIconDrawer.Title = "图标ID";
        SkillIconDrawer.EndEdit = num => SelectedItem.Icon = num;
        
        SkillEffectDrawer.Title = "技能特效";
        SkillEffectDrawer.EndEdit = str => SelectedItem.SkillEffect = str;

        AffixDrawer.Title = "词缀列表";
        AffixDrawer.EndEdit = str =>
        {
            if (str.TryFormatToListInt(out var list))
            {
                SelectedItem.AffixList = list;
                AffixDrawer.HideWarning();
                AffixDrawer.Content = list.ToFormatString();
            }
            else
            {
                AffixDrawer.ShowWarning("");
            }
            CommonEditor.RefreshItem(SelectedItem);
        };

        AffixPreviewDrawer.Title = "词缀预览";

        BuffTypeDrawer.Title = "Buff类型";
        BuffTypeDrawer.SetOptions(ModMgr.Instance.BuffDataBuffTypes.Select(item => $"{item.TypeID} {item.TypeName}").ToList());
        BuffTypeDrawer.ValueChange = index =>
        {
            var buffType = ModMgr.Instance.BuffDataBuffTypes[index];
            SelectedItem.BuffType = buffType.TypeID;
        };

        BuffTriggerTypeDrawer.Title = "触发类型";
        BuffTriggerTypeDrawer.SetOptions(ModMgr.Instance.BuffDataTriggerTypes.Select(item => $"{item.ID} {item.Desc}").ToList());
        BuffTriggerTypeDrawer.ValueChange = index =>
        {
            var buffTrigger = ModMgr.Instance.BuffDataTriggerTypes[index];
            SelectedItem.Trigger = buffTrigger.ID;
        };

        BuffRemoveTriggerTypeDrawer.Title = "移除类型";
        BuffRemoveTriggerTypeDrawer.SetOptions(ModMgr.Instance.BuffDataRemoveTriggerTypes.Select(item => $"{item.ID} {item.Desc}").ToList());
        BuffRemoveTriggerTypeDrawer.ValueChange = index =>
        {
            var buffRemoveTrigger = ModMgr.Instance.BuffDataRemoveTriggerTypes[index];
            SelectedItem.RemoverTrigger = buffRemoveTrigger.ID;
        };
        
        BuffOverlayTypeDrawer.Title = "叠加类型";
        BuffOverlayTypeDrawer.SetOptions(ModMgr.Instance.BuffDataOverlayTypes.Select(item => $"{item.ID} {item.Desc}").ToList());
        BuffOverlayTypeDrawer.ValueChange = index =>
        {
            var buffOverlayType = ModMgr.Instance.BuffDataOverlayTypes[index];
            SelectedItem.RemoverTrigger = buffOverlayType.ID;
        };

        BuffHideDrawer.Title = "是否隐藏";
        BuffHideDrawer.OnValueChange = isOn => SelectedItem.IsHide = isOn ? 1 : 0;

        BuffShowOnlyOneDrawer.Title = "只显示一层";
        BuffShowOnlyOneDrawer.OnValueChange = isOn => SelectedItem.ShowOnlyOne = isOn ? 1 : 0;

        SeidListDrawer.Title = "特性";
        SeidListDrawer.BindSeid(this,
            () => ModMgr.Instance.BuffSeidMetas,
            () => BindProject.BuffSeidDataGroup,
            () => SelectedItem.SeidList);
        
        OuterSeidListDrawer.Title = "未加入特性";
        OuterSeidListDrawer.BindSeid(this,
            () => ModMgr.Instance.BuffSeidMetas,
            () => BindProject.BuffSeidDataGroup,
            () =>
            {
                var list = new List<int>();
                foreach (var pair in BindProject.BuffSeidDataGroup.DataGroups)
                {
                    var seid = BindProject.BuffSeidDataGroup.GetSeid(SelectedItem.ID, pair.Key);
                    if (seid != null && !SelectedItem.SeidList.Contains(pair.Key))
                    {
                        list.Add(pair.Key);
                    }
                }
                return list;
            });
        OuterSeidListDrawer.CanDrag = false;
        OuterSeidListDrawer.ChangeApplyToSeidList = false;
        
        BuffDescDrawer.Title = "描述";
        BuffDescDrawer.EndEdit = str => SelectedItem.Desc = str;
    }

    protected override void OnEditorRefresh(IModData data)
    {
        var curData = (ModBuffData)data;
        IdDrawer.Content = curData.ID.ToString();
        NameDrawer.Content = curData.Name;
        SkillIconDrawer.Content = curData.Icon.ToString();
        SkillEffectDrawer.Content = curData.SkillEffect;
        AffixDrawer.Content = curData.AffixList.ToFormatString();
        if (curData.AffixList.Count == 0)
        {
            AffixPreviewDrawer.gameObject.SetActive(false);
        }
        else
        {
            AffixPreviewDrawer.gameObject.SetActive(true);
            var sb = new StringBuilder();
            foreach (var id in curData.AffixList)
            {
                var affixData = BindProject.FindAffix(id);
                sb.Append(ModUtils.GetAffixDesc(affixData));
                sb.Append("\n");
            }

            AffixPreviewDrawer.Content = sb.ToString();
        }
        BuffTypeDrawer.Select(ModMgr.Instance.BuffDataBuffTypes
            .TryFind(item => item.TypeID == curData.BuffType));
        BuffTriggerTypeDrawer.Select(ModMgr.Instance.BuffDataTriggerTypes
            .TryFind(item => item.ID == curData.Trigger));
        BuffRemoveTriggerTypeDrawer.Select(ModMgr.Instance.BuffDataRemoveTriggerTypes
            .TryFind(item => item.ID == curData.RemoverTrigger));
        BuffOverlayTypeDrawer.Select(ModMgr.Instance.BuffDataOverlayTypes
            .TryFind(item => item.ID == curData.BuffOverlayType));
        BuffHideDrawer.IsOn = SelectedItem.IsHide == 1;
        BuffShowOnlyOneDrawer.IsOn = SelectedItem.ShowOnlyOne == 1;
        SeidListDrawer.Refresh(meta=>$"{meta.ID} {meta.Name}");
        OuterSeidListDrawer.Refresh(meta=>$"{meta.ID} {meta.Name}");
        BuffDescDrawer.Content = curData.Desc;
    }

    protected override bool OnFilterData(IModData data, string filter)
    {
        var buffData = (ModBuffData)data;
        var flag = buffData.ID.ToString().Contains(filter) ||
            buffData.Name.Contains(filter) ||
            buffData.Desc.Contains(filter);
        return flag;
    }

    protected override string GetItemName(IModData data)
    {
        var buffData = (ModBuffData)data;
        return $"{buffData.ID} {buffData.Name}";
    }
}