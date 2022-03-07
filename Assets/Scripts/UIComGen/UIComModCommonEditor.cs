/*
 * UIComModCommonEditor
 * This code is generated by code. Do not modify it。
 */
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using VirtualList;

public partial class UIComModCommonEditor : UIComBase
{
    public static string ComName => "ModCommonEditor";

    [NonSerialized]
    public ScrollRect lstTabs;
    [NonSerialized]
    public AbstractVirtualList vlstItems;
    [NonSerialized]
    public RectTransform goEditorDrawers;
    [NonSerialized]
    public RectTransform goDrawerRoot;
    [NonSerialized]
    public Button btnItemAdd;
    [NonSerialized]
    public TextMeshProUGUI txtItemAdd;
    [NonSerialized]
    public Button btnItemRemove;
    [NonSerialized]
    public TextMeshProUGUI txtItemRemove;

    protected override void OnPreInit()
    {
        lstTabs = FindBindComponent<ScrollRect>("g:lstTabs");
        vlstItems = FindBindComponent<AbstractVirtualList>("g:vlstItems");
        goEditorDrawers = FindBindComponent<RectTransform>("g:goEditorDrawers");
        goDrawerRoot = FindBindComponent<RectTransform>("g:goDrawerRoot");
        btnItemAdd = FindBindComponent<Button>("g:btnItemAdd");
        txtItemAdd = FindBindComponent<TextMeshProUGUI>("g:txtItemAdd");
        btnItemRemove = FindBindComponent<Button>("g:btnItemRemove");
        txtItemRemove = FindBindComponent<TextMeshProUGUI>("g:txtItemRemove");

    }
}