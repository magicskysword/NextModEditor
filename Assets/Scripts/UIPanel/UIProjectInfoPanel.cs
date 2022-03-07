using System.Collections.Generic;
using UniRx;
using UnityEngine.UI;

public partial class UIProjectInfoPanel
{
    private ModProject _project;
    private int _selectedEditorIndex;
    
    public ModProject Project
    {
        get => _project;
        set
        {
            _project = value;
            RefreshProjectInfo();
            RefreshEditor();
        }
    }
    public List<IModEditor> Editors { get; set; } = new List<IModEditor>();
    public List<UIComTglModPanel> EditorOptions { get; set; } = new List<UIComTglModPanel>();
    public int SelectedEditorIndex
    {
        get => _selectedEditorIndex;
        set
        {
            _selectedEditorIndex = value;
            RefreshEditor();
        } 
    }

    protected override void OnInit()
    {
        EventCenter.AsObservable<LoadModProjectEventArgs>()
            .Subscribe(args =>
            {
                Project = args.ModProject;
                EditorOptions[0].tglTab.isOn = true;
            })
            .AddTo(this);

        CreateEditor<UIModConfigEditorPanel>("Mod配置");
        CreateEditor<UIModAffixEditorPanel>("词缀");
        CreateEditor<UIModCreateAvatarEditorPanel>("创建角色天赋");
        CreateEditor<UIModBuffEditorPanel>("Buff数据");

        Project = ModMgr.Instance.CurProject;

        EditorOptions[0].tglTab.isOn = true;
    }

    private void CreateEditor<T>(string editorName) where T : UIPanelBase, IModEditor
    {
        var panel = UIMgr.Instance.GetPanel<T>(UILayer.Middle);
        var tab = UIMgr.Instance.CreateCom<UIComTglModPanel>(goTabRoot);
        var panelIndex = Editors.Count;
        
        EditorOptions.Add(tab);
        Editors.Add(panel);
        
        panel.Show();
        
        tab.txtTab.text = editorName;
        tab.tglTab.group = tglGrpProjectTabs;
        tab.tglTab.OnValueChangedAsObservable()
            .Subscribe(isOn =>
            {
                if (isOn)
                {
                    SelectedEditorIndex = panelIndex;
                }
            });
    }

    private void RefreshEditor()
    {
        bool hideAllEditor = Project == null;
        
        for (var index = 0; index < Editors.Count; index++)
        {
            var editor = Editors[index];
            var editorPanel = (UIPanelBase)editor;
            if (hideAllEditor || SelectedEditorIndex != index)
            {
                editorPanel.Hide();
            }
            else
            {
                editor.BindProject = Project;
                editorPanel.Show();
            }
        }
    }

    private void RefreshProjectInfo()
    {
        if (Project == null)
        {
            txtCurProject.text = $"未打开工程";
        }
        else
        {
            txtCurProject.text = $"当前工程：{Project.ProjectPath}";
        }
    }
}