using UniRx;
using UnityEngine;

public partial class UIMainHeaderPanel
{
    protected override void OnInit()
    {
        txtVersion.text = $"Next Mod编辑器 v{ModMgr.EDITOR_VERSION}";
        
        EventCenter.AsObservable<LoadModProjectEventArgs>()
            .Subscribe(args =>
            {
                RefreshHeadBar(args.ModProject);
            })
            .AddTo(this);
        
        btnCmdOpen.OnClickAsObservable().Subscribe(_ =>
        {
            var project = ModMgr.Instance.OpenProject();
            if (project != null)
            {
                ModMgr.Instance.CurProject = project;
                EventCenter.Send(new LoadModProjectEventArgs()
                {
                    ModProject = project
                });
            }
        });
        
        btnCmdSave.OnClickAsObservable().Subscribe(_ =>
        {
            var project = ModMgr.Instance.CurProject;
            if(project != null)
            {
                ModMgr.Instance.SaveProject(project);
                UIMessageBoxPanel.ShowMessage("保存成功", $"项目已保存完毕。");
            }
        });
        
        btnCmdAbout.OnClickAsObservable().Subscribe(_ =>
        {
            UIMgr.Instance.GetPanel<UIAboutPanel>(UILayer.Model).Show();
        });
        
        goCmdRoot.ForceUpdateRectTransforms();
    }

    private void RefreshHeadBar(ModProject argsModProject)
    {
        btnCmdSave.interactable = argsModProject != null;
    }
}