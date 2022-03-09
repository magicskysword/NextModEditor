using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public partial class UIModAffixEditorPanel : UIModCommonEditorFramePanel
{
    #region Drawer

    public UIComInputIdDrawer IdDrawer { get; set; }
    public UIComDropdownDrawer ProjectTypeDrawer { get; set; }
    public UIComDropdownDrawer TypeDrawer { get; set; }
    public UIComInputTextAreaDrawer DescDrawer { get; set; }

    #endregion
    public ModAffixData SelectedItem => (ModAffixData)SelectModData;
    public override IList DataList => BindProject.AffixData;

    protected override void OnInitEditor()
    {
        IdDrawer = CommonEditor.AddEditorDrawer<UIComInputIdDrawer>();
        ProjectTypeDrawer = CommonEditor.AddEditorDrawer<UIComDropdownDrawer>();
        TypeDrawer = CommonEditor.AddEditorDrawer<UIComDropdownDrawer>();
        DescDrawer = CommonEditor.AddEditorDrawer<UIComInputTextAreaDrawer>();
        
        IdDrawer.Title = "ID";
        IdDrawer.BindEditor(this,
            ()=>BindProject.AffixData,
            data =>
            {
                var curData = (ModAffixData)data;
                return $"{curData.ID} {curData.Name}";
            },
            (data,newId) =>
            {
                SelectedItem.ID = newId;
                BindProject.AffixData.ModSort();
                CommonEditor.RefreshItem(SelectedItem);
            },
            (data,otherData) =>
            {
                (otherData.ID, data.ID) = (data.ID, otherData.ID);
                BindProject.AffixData.ModSort();
                CommonEditor.RefreshItem(SelectedItem);
            },
            () =>
            {
                IdDrawer.Content = SelectedItem.ID.ToString();
            });

        ProjectTypeDrawer.Title = "词缀项目";
        ProjectTypeDrawer.SetOptions(ModMgr.Instance.AffixDataProjectTypes.Select(item => $"{item.TypeNum} {item.TypeName}").ToList());
        ProjectTypeDrawer.ValueChange = index =>
        {
            var projectType = ModMgr.Instance.AffixDataProjectTypes[index];
            SelectedItem.SetProjectType(projectType);
        };
        
        TypeDrawer.Title = "词缀类型";
        TypeDrawer.SetOptions(ModMgr.Instance.AffixDataAffixTypes.Select(item => $"{item.ID} {item.Name}").ToList());
        TypeDrawer.ValueChange = index =>
        {
            var projectType = ModMgr.Instance.AffixDataAffixTypes[index];
            SelectedItem.AffixType = projectType.ID;
        };
        
        DescDrawer.Title = "描述";
        DescDrawer.EndEdit = str => SelectedItem.Desc = str;
    }

    protected override void OnEditorRefresh(IModData data)
    {
        var curData = (ModAffixData)data;
        IdDrawer.Content = curData.ID.ToString();
        ProjectTypeDrawer.Select(ModMgr.Instance.AffixDataProjectTypes
            .TryFind(item => item.TypeNum == curData.ProjectTypeNum));
        TypeDrawer.Select(ModMgr.Instance.AffixDataAffixTypes
            .TryFind(item => item.ID == curData.AffixType));
        DescDrawer.Content = curData.Desc;
    }

    protected override bool OnFilterData(IModData data, string filter)
    {
        var affixData = (ModAffixData)data;
        var flag = affixData.ID.ToString().Contains(filter) ||
                   affixData.Name.Contains(filter) ||
                   affixData.Desc.Contains(filter);
        return flag;
    }

    protected override string GetItemName(IModData data)
    {
        var affixData = (ModAffixData)data;
        return $"{affixData.ID} {affixData.Name}";
    }
}