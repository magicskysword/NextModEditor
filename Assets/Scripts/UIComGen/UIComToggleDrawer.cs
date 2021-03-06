/*
 * UIComToggleDrawer
 * This code is generated by code. Do not modify it。
 */
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using VirtualList;

public partial class UIComToggleDrawer : UIComBase
{
    public static string ComName => "toggleDrawer";

    [NonSerialized]
    public TextMeshProUGUI txtTitle;
    [NonSerialized]
    public Toggle tglMain;

    protected override void OnPreInit()
    {
        txtTitle = FindBindComponent<TextMeshProUGUI>("g:txtTitle");
        tglMain = FindBindComponent<Toggle>("g:tglMain");

    }
}