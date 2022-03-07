using System.Linq;
using System.Text;

public partial class UIModAffixEditorPanel : IModDataEditor
{
    private ModProject _bindProject;
    private ModAffixData _selectedItem;

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

    public ModAffixData SelectedItem
    {
        get => _selectedItem;
        set
        {
            _selectedItem = value;
            CommonEditor.RefreshItem(_selectedItem);
        }
    }
    
    public UIComModCommonEditor CommonEditor { get; set; }
    public ModDataListSource<ModAffixData> DataListSource { get; set; }

    #region Drawer

    public UIComInputIdDrawer IdDrawer { get; set; }
    public UIComDropdownDrawer ProjectTypeDrawer { get; set; }
    public UIComDropdownDrawer TypeDrawer { get; set; }
    public UIComInputTextAreaDrawer DescDrawer { get; set; }

    #endregion
        
    protected override void OnInit()
    {
        CommonEditor = UIMgr.Instance.CreateCom<UIComModCommonEditor>(transform);
        
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
        
        // 刷新所有Drawer
        CommonEditor.OnRefreshItem = data =>
        {
            var curData = (ModAffixData)data;
            IdDrawer.Content = curData.ID.ToString();
            ProjectTypeDrawer.Select(ModMgr.Instance.AffixDataProjectTypes
                .TryFind(item => item.TypeNum == curData.ProjectTypeNum));
            TypeDrawer.Select(ModMgr.Instance.AffixDataAffixTypes
                .TryFind(item => item.ID == curData.AffixType));
            DescDrawer.Content = curData.Desc;
            
            var dataIndex = BindProject.AffixData.FindIndex(searchData => searchData == data);
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
                        BindProject.AffixData.Remove(SelectedItem);
                        RefreshUI();
                    });
            }
        };
        
        CommonEditor.OnClickAddItem = () =>
        {
            var maxIndex = BindProject.AffixData.Count > 0 
                ? BindProject.AffixData.Max(item => item.ID)
                : 0;
            var newData = new ModAffixData()
            {
                ID = maxIndex + 1
            };
            BindProject.AffixData.Add(newData);
            SelectedItem = newData;
        };
        
        DataListSource = new ModDataListSource<ModAffixData>();
        DataListSource.RendererItem += (item,data) => item.txtTab.text = $"{data.ID} {data.Name}";;
        DataListSource.SelectData += item => SelectedItem = item;
    }
    
    private void RefreshUI()
    {
        DataListSource.DataList = BindProject.AffixData;
        DataListSource.SelectedIndex = 0;
        CommonEditor.SetItemList(DataListSource);
        if (BindProject.AffixData.Count > 0)
        {
            SelectedItem = BindProject.AffixData[0];
        }
        else
        {
            SelectedItem = null;
        }
    }
}