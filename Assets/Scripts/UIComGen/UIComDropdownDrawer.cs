/*
 * UIComDropdownDrawer
 * This code is generated by code. Do not modify it。
 */
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using VirtualList;

public partial class UIComDropdownDrawer : UIComBase
{
    public static string ComName => "dropdownDrawer";

    [NonSerialized]
    public TextMeshProUGUI txtTitle;
    [NonSerialized]
    public TMP_Dropdown ddlMain;

    protected override void OnPreInit()
    {
        txtTitle = FindBindComponent<TextMeshProUGUI>("g:txtTitle");
        ddlMain = FindBindComponent<TMP_Dropdown>("g:ddlMain");

    }
}