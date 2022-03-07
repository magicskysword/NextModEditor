/*
 * UIComListDrawer
 * This code is generated by code. Do not modify it。
 */
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using VirtualList;
using UnityEngine.UI.Extensions;

public partial class UIComListDrawer : UIComBase
{
    public static string ComName => "listDrawer";

    [NonSerialized]
    public TextMeshProUGUI txtTitle;
    [NonSerialized]
    public ReorderableList rolstMain;
    [NonSerialized]
    public Button btnAdd;

    protected override void OnPreInit()
    {
        txtTitle = FindBindComponent<TextMeshProUGUI>("g:txtTitle");
        rolstMain = FindBindComponent<ReorderableList>("g:rolstMain");
        btnAdd = FindBindComponent<Button>("g:btnAdd");

    }
}