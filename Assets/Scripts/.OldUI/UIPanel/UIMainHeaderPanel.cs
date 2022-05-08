using System;
using System.Diagnostics;
using UniRx;
using Debug = UnityEngine.Debug;

public partial class UIMainHeaderPanel
{
    protected override void OnInit()
    {
        txtVersion.text = $"Next Mod编辑器 v{BootstrapOld.EDITOR_VERSION}";
        
        EventCenter.AsObservable<LoadModProjectEventArgs>()
            .Subscribe(args =>
            {
                RefreshHeadBar(args.ModProject);
            })
            .AddTo(this);
        
        btnCmdOpen.OnClickAsObservable().Subscribe(_ =>
        {
            try
            {
                var project = ModMgr.I.OpenProject();
                if (project != null)
                {
                    ModMgr.I.CurProject = project;
                    EventCenter.Send(new LoadModProjectEventArgs()
                    {
                        ModProject = project
                    });
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                UIMessageScrollBoxPanel.ShowMessage($"警告",$"加载Mod发生错误！请检查配置数据或Mod数据。\n{e}"
                    ,callback: () =>
                    {
                        ModMgr.I.CurProject = null;
                        EventCenter.Send(new LoadModProjectEventArgs()
                        {
                            ModProject = null
                        });
                    });
            }
        });
        
        btnCmdSave.OnClickAsObservable().Subscribe(_ =>
        {
            try
            {
                var project = ModMgr.I.CurProject;
                if(project != null)
                {
                    ModMgr.I.SaveProject(project);
                    UIMessageBoxPanel.ShowMessage("保存成功", $"项目已保存完毕。");
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                UIMessageScrollBoxPanel.ShowMessage($"警告",$"保存Mod发生错误！请检查配置数据并使用安全模式导出。\n{e}"
                    ,callback: () =>
                    {
                        ModMgr.I.CurProject = null;
                        EventCenter.Send(new LoadModProjectEventArgs()
                        {
                            ModProject = null
                        });
                    });
            }
        });

        btnCmdFolder.OnClickAsObservable().Subscribe(_ =>
        {
            Process.Start(ModMgr.I.CurProject.ProjectPath);
        });
        
        btnCmdAbout.OnClickAsObservable().Subscribe(_ =>
        {
            UIMgr.Instance.GetPanel<UIAboutPanel>(UILayer.Model).Show();
        });
        
        goCmdRoot.ForceUpdateRectTransforms();
        
        UnityEngine.Canvas.ForceUpdateCanvases();
    }

    private void RefreshHeadBar(ModProject argsModProject)
    {
        btnCmdSave.interactable = argsModProject != null;
        btnCmdFolder.interactable = argsModProject != null;
    }
}