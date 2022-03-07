/*
 * UICreateSeidBoxPanel
 * This code is generated by code. Do not modify it。
 */
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using VirtualList;

[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(GraphicRaycaster))]
public partial class UICreateSeidBoxPanel : UIPanelBase
{
    public static string PanelName => "CreateSeidBox";

    [NonSerialized]
    public TextMeshProUGUI txtTitle;
    [NonSerialized]
    public TextMeshProUGUI txtContent;
    [NonSerialized]
    public TMP_Dropdown ddlOption;
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
    public RectTransform goCustomInput;
    [NonSerialized]
    public TMP_InputField inMain;
    [NonSerialized]
    public RectTransform imgWarning;

    protected override void OnPreInit()
    {
        txtTitle = FindBindComponent<TextMeshProUGUI>("g:txtTitle");
        txtContent = FindBindComponent<TextMeshProUGUI>("g:txtContent");
        ddlOption = FindBindComponent<TMP_Dropdown>("g:ddlOption");
        btnOk = FindBindComponent<Button>("g:btnOk");
        txtOk = FindBindComponent<TextMeshProUGUI>("g:txtOk");
        btnCancel = FindBindComponent<Button>("g:btnCancel");
        txtCancel = FindBindComponent<TextMeshProUGUI>("g:txtCancel");
        tglUseCustomValue = FindBindComponent<Toggle>("g:tglUseCustomValue");
        goCustomInput = FindBindComponent<RectTransform>("g:goCustomInput");
        inMain = FindBindComponent<TMP_InputField>("g:inMain");
        imgWarning = FindBindComponent<RectTransform>("g:imgWarning");

    }
}