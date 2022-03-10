using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    public const string EDITOR_VERSION = "0.1.0";
    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        try
        {
            ModMgr.Instance.Init();

            UIMgr.Instance.GetPanel<UIBackgroundPanel>(UILayer.Bottom).Show();
            UIMgr.Instance.GetPanel<UIMainHeaderPanel>(UILayer.Middle).Show();
            UIMgr.Instance.GetPanel<UIProjectInfoPanel>(UILayer.Middle).Show();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            UIMessageScrollBoxPanel.ShowMessage($"警告",$"初始化发生错误！请检查配置数据或重装编辑器。\n{e}"
            ,callback: Application.Quit);
        }
    }
}
