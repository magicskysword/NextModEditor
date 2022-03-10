using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
 
 public partial class UIModCreateAvatarEditorPanel : UIModCommonEditorFramePanel
 {
     #region Drawer
 
     public UIComInputIdDrawer IdDrawer { get; set; }
     public UIComInputTextDrawer NameDrawer { get; set; }
     public UIComInputNumberDrawer GroupDrawer { get; set; }
     public UIComInputNumberDrawer CostDrawer { get; set; }
     public UIComDropdownDrawer CreateTypeDrawer { get; set; }
     public UIComSeidListDrawer SeidListDrawer { get; set; }
     public UIComSeidListDrawer OuterSeidListDrawer { get; set; }
     public UIComDropdownDrawer RequireLevelDrawer { get; set; }
     public UIComInputTextAreaDrawer DescDrawer { get; set; }
     public UIComInputTextAreaDrawer InfoDrawer { get; set; }
 
     #endregion
     public ModCreateAvatarData SelectedItem => (ModCreateAvatarData)SelectModData;
     public override IList DataList => BindProject.CreateAvatarData;
     public override Type ItemType => typeof(ModCreateAvatarData);

     protected override void OnInitEditor()
     {
         IdDrawer = CommonEditor.AddEditorDrawer<UIComInputIdDrawer>();
         NameDrawer = CommonEditor.AddEditorDrawer<UIComInputTextDrawer>();
         GroupDrawer = CommonEditor.AddEditorDrawer<UIComInputNumberDrawer>();
         CostDrawer = CommonEditor.AddEditorDrawer<UIComInputNumberDrawer>();
         CreateTypeDrawer = CommonEditor.AddEditorDrawer<UIComDropdownDrawer>();
         SeidListDrawer = CommonEditor.AddEditorDrawer<UIComSeidListDrawer>();
         OuterSeidListDrawer = CommonEditor.AddEditorDrawer<UIComSeidListDrawer>();
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
         
         SeidListDrawer.Title = "特性";
         SeidListDrawer.BindSeid(this,
             () => ModMgr.Instance.CreateAvatarSeidMetas,
             () => BindProject.CreateAvatarSeidDataGroup,
             () => SelectedItem.SeidList);

         OuterSeidListDrawer.Title = "未加入特性";
         OuterSeidListDrawer.BindSeid(this,
             () => ModMgr.Instance.CreateAvatarSeidMetas,
             () => BindProject.CreateAvatarSeidDataGroup,
             () =>
             {
                 var list = new List<int>();
                 foreach (var pair in BindProject.CreateAvatarSeidDataGroup.DataGroups)
                 {
                     var seid = BindProject.CreateAvatarSeidDataGroup.GetSeid(SelectedItem.ID, pair.Key);
                     if (seid != null && !SelectedItem.SeidList.Contains(pair.Key))
                     {
                         list.Add(pair.Key);
                     }
                 }
                 return list;
             });
         OuterSeidListDrawer.CanDrag = false;
         OuterSeidListDrawer.ChangeApplyToSeidList = false;
         
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
     }

     protected override void OnEditorRefresh(IModData data)
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
         OuterSeidListDrawer.Refresh(meta=>$"{meta.ID} {meta.Name}");
     }

     protected override bool OnFilterData(IModData data, string filter)
     {
         var createAvatarData = (ModCreateAvatarData)data;
         var flag = createAvatarData.ID.ToString().Contains(filter) ||
             createAvatarData.Name.Contains(filter) ||
             createAvatarData.Desc.Contains(filter);
         return flag;
     }

     protected override string GetItemName(IModData data)
     {
         var createAvatarData = (ModCreateAvatarData)data;
         return $"{createAvatarData.ID} {createAvatarData.Name}";
     }
 }