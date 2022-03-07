using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        ModMgr.Instance.Init();

        UIMgr.Instance.GetPanel<UIBackgroundPanel>(UILayer.Bottom).Show();
        UIMgr.Instance.GetPanel<UIMainHeaderPanel>(UILayer.Middle).Show();
        UIMgr.Instance.GetPanel<UIProjectInfoPanel>(UILayer.Middle).Show();
    }
}
