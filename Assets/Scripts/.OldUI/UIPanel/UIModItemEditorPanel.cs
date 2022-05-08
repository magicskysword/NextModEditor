using System;
using System.Collections;
using System.Linq;

public class UIModItemEditorPanel : UIModCommonEditorFramePanel
{
    public ModItemData SelectedItem => (ModItemData)SelectModData;
    public override IList DataList => BindProject.ItemData;
    public override Type ItemType => typeof(ModItemData);

    #region Drawer

    public UIComInputIdDrawer IdDrawer { get; set; }
    public UIComInputTextDrawer NameDrawer { get; set; }
    public UIComInputNumberDrawer IconDrawer { get; set; }
    public UIComInputNumberDrawer MaxNumDrawer { get; set; }
    public UIComInputTextDrawer ArtifactTypeDrawer { get; set; }
    public UIComInputTextDrawer AffixDrawer { get; set; }
    public UIComTextWithContentDrawer AffixPreviewDrawer { get; set; }
    public UIComDropdownDrawer GuideTypeDrawer { get; set; }
    public UIComInputNumberDrawer ShopTypeDrawer { get; set; }

    #endregion
    
    protected override void OnInitEditor()
    {
        IdDrawer = CommonEditor.AddEditorDrawer<UIComInputIdDrawer>();
        NameDrawer = CommonEditor.AddEditorDrawer<UIComInputTextDrawer>();
        IconDrawer = CommonEditor.AddEditorDrawer<UIComInputNumberDrawer>();
        MaxNumDrawer = CommonEditor.AddEditorDrawer<UIComInputNumberDrawer>();
        ArtifactTypeDrawer = CommonEditor.AddEditorDrawer<UIComInputTextDrawer>();
        AffixDrawer = CommonEditor.AddEditorDrawer<UIComInputTextDrawer>();
        AffixPreviewDrawer = CommonEditor.AddEditorDrawer<UIComTextWithContentDrawer>();
        GuideTypeDrawer = CommonEditor.AddEditorDrawer<UIComDropdownDrawer>();
        ShopTypeDrawer = CommonEditor.AddEditorDrawer<UIComInputNumberDrawer>();
        
        IdDrawer.Title = "ID";
        IdDrawer.BindEditor(this,
            ()=>BindProject.BuffData,
            data =>
            {
                var curData = (ModItemData)data;
                return $"{curData.ID} {curData.Name}";
            },
            (data,newId) =>
            {
                data.ID = newId;
                BindProject.ItemData.ModSort();
                CommonEditor.RefreshItem(data);
            },
            (data,otherData) =>
            {
                (otherData.ID, data.ID) = (data.ID, otherData.ID);
                BindProject.ItemData.ModSort();
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

        IconDrawer.Title = "图标ID";
        IconDrawer.EndEdit = num => SelectedItem.ItemIcon = num;
        
        MaxNumDrawer.Title = "最大堆叠";
        MaxNumDrawer.EndEdit = num => SelectedItem.MaxStack = num;

        ArtifactTypeDrawer.Title = "法宝类型";
        ArtifactTypeDrawer.EndEdit = txt => SelectedItem.ArtifactType = txt;
        
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

        GuideTypeDrawer.Title = "图鉴类型";
        GuideTypeDrawer.SetOptions(ModMgr.I.ItemDataGuideTypes.Select(item => $"{item.ID} {item.Desc}").ToList());
        GuideTypeDrawer.ValueChange = index =>
        {
            var guideType = ModMgr.I.ItemDataGuideTypes[index];
            SelectedItem.GuideType = guideType.ID;
        };

        ShopTypeDrawer.Title = "商店类型";
        ShopTypeDrawer.EndEdit = num => SelectedItem.ShopType = num;
    }

    protected override void OnEditorRefresh(IModData data)
    {
        var curData = (ModItemData)data;
        IdDrawer.Content = curData.ID.ToString();
        NameDrawer.Content = curData.Name;
        IconDrawer.Content = curData.ItemIcon.ToString();
        MaxNumDrawer.Content = curData.MaxStack.ToString();
        ArtifactTypeDrawer.Content = curData.ArtifactType;
        AffixDrawer.Content = curData.AffixList.ToFormatString();
        if (curData.AffixList.Count == 0)
        {
            AffixPreviewDrawer.gameObject.SetActive(false);
        }
        else
        {
            AffixPreviewDrawer.gameObject.SetActive(true);
            AffixPreviewDrawer.Content = ModUtils.GetAffixDesc(curData.AffixList);
        }
        GuideTypeDrawer.Select(ModMgr.I.ItemDataGuideTypes
            .TryFind(item => item.ID == curData.GuideType));
        ShopTypeDrawer.Content = curData.ShopType.ToString();
    }

    protected override bool OnFilterData(IModData data, string filter)
    {
        var itemData = (ModItemData)data;
        var flag = itemData.ID.ToString().Contains(filter) ||
                   itemData.Name.Contains(filter) ||
                   itemData.Desc.Contains(filter) ||
                   itemData.Info.Contains(filter);
        return flag;
    }

    protected override string GetItemName(IModData data)
    {
        var itemData = (ModItemData)data;
        return $"{itemData.ID} {itemData.Name}";
    }
}