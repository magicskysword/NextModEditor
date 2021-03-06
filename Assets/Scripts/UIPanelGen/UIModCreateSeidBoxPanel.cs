/*
 * UIModCreateSeidBoxPanel
 * This code is generated by code. Do not modify it。
 */
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using VirtualList;
using UnityEngine.UI.Extensions;

[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(GraphicRaycaster))]
public partial class UIModCreateSeidBoxPanel : UIPanelBase
{
    public static string PanelName => "ModCreateSeidBox";

    [NonSerialized]
    public TextMeshProUGUI txtTitle;
    [NonSerialized]
    public TextMeshProUGUI txtContent;
    [NonSerialized]
    public Button btnOk;
    [NonSerialized]
    public TextMeshProUGUI txtOk;
    [NonSerialized]
    public Button btnCancel;
    [NonSerialized]
    public TextMeshProUGUI txtCancel;
    [NonSerialized]
    public Toggle tglUseCustomValue;
    [NonSerialized]
    public RectTransform goCustomRoot;
    [NonSerialized]
    public RectTransform goCustomInput;
    [NonSerialized]
    public TMP_InputField inCustomId;
    [NonSerialized]
    public RectTransform imgWarning;
    [NonSerialized]
    public TextMeshProUGUI txtCustomDesc;
    [NonSerialized]
    public RectTransform goListRoot;
    [NonSerialized]
    public ScrollRect lstTabs;
    [NonSerialized]
    public AbstractVirtualList vlstItems;
    [NonSerialized]
    public TMP_InputField inSearch;

    protected override void OnPreInit()
    {
        txtTitle = FindBindComponent<TextMeshProUGUI>("g:txtTitle");
        txtContent = FindBindComponent<TextMeshProUGUI>("g:txtContent");
        btnOk = FindBindComponent<Button>("g:btnOk");
        txtOk = FindBindComponent<TextMeshProUGUI>("g:txtOk");
        btnCancel = FindBindComponent<Button>("g:btnCancel");
        txtCancel = FindBindComponent<TextMeshProUGUI>("g:txtCancel");
        tglUseCustomValue = FindBindComponent<Toggle>("g:tglUseCustomValue");
        goCustomRoot = FindBindComponent<RectTransform>("g:goCustomRoot");
        goCustomInput = FindBindComponent<RectTransform>("g:goCustomInput");
        inCustomId = FindBindComponent<TMP_InputField>("g:inCustomId");
        imgWarning = FindBindComponent<RectTransform>("g:imgWarning");
        txtCustomDesc = FindBindComponent<TextMeshProUGUI>("g:txtCustomDesc");
        goListRoot = FindBindComponent<RectTransform>("g:goListRoot");
        lstTabs = FindBindComponent<ScrollRect>("g:lstTabs");
        vlstItems = FindBindComponent<AbstractVirtualList>("g:vlstItems");
        inSearch = FindBindComponent<TMP_InputField>("g:inSearch");

    }
}