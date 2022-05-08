using System;
using UIPkg.Main;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    public const string EDITOR_VERSION = "0.2.0";
    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = Screen.currentResolution.refreshRate;

        try
        {
            ModMgr.I.Init();
            FGUIMgr.I.Init();

            FGUIMgr.I.ShowPanel<UIPanelMain>();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }
        
}