using System;
using UniRx;
using UnityEngine;

public partial class UIMainHeaderPanel
{
    protected override void OnInit()
    {
        txtVersion.text = $"Next Mod编辑器 v{Bootstrap.EDITOR_VERSION}";
        
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
                var project = ModMgr.Instance.OpenProject();
                if (project != null)
                {
                    ModMgr.Instance.CurProject = project;
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
                        ModMgr.Instance.CurProject = null;
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
                var project = ModMgr.Instance.CurProject;
                if(project != null)
                {
                    ModMgr.Instance.SaveProject(project);
                    UIMessageBoxPanel.ShowMessage("保存成功", $"项目已保存完毕。");
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                UIMessageScrollBoxPanel.ShowMessage($"警告",$"保存Mod发生错误！请检查配置数据并使用安全模式导出。\n{e}"
                    ,callback: () =>
                    {
                        ModMgr.Instance.CurProject = null;
                        EventCenter.Send(new LoadModProjectEventArgs()
                        {
                            ModProject = null
                        });
                    });
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