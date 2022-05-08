using System;
using System.Collections.Generic;
using UniRx;

public partial class UICreateSeidBoxPanel
{
    public int DropdownIndex => ddlOption.value;
    public int CustomNumber => int.TryParse(inMain.text, out var num) ? num : -1;

    public bool UseCustomNumber
    {
        get => tglUseCustomValue.isOn;
        set
        {
            tglUseCustomValue.SetIsOnWithoutNotify(value);
            if (value)
            {
                // Custom Number
                goCustomInput.gameObject.SetActive(true);
                ddlOption.gameObject.SetActive(false);
            }
            else
            {
                // Dropdown List
                goCustomInput.gameObject.SetActive(false);
                ddlOption.gameObject.SetActive(true);
            }
        }
    }

    protected override void OnInit()
    {
        goCustomInput.gameObject.SetActive(false);
        inMain.text = "0";
        imgWarning.gameObject.SetActive(false);
    }

    public void SetOptions(List<string> options)
    {
        ddlOption.ClearOptions();
        ddlOption.AddOptions(options);
    }

    public static void ShowMessage(string title, string content, List<string> options,
        string okText = "确定", string cancelText = "取消", 
        Action<UICreateSeidBoxPanel> onValueChange = null,Action<UICreateSeidBoxPanel> onOk = null, Action onCancel = null)
    {
        var msgBox = UIMgr.Instance.GetPanel<UICreateSeidBoxPanel>(UILayer.Model);
        msgBox.ddlOption.OnValueChangedAsObservable().Subscribe(value =>
        {
            onValueChange?.Invoke(msgBox);
        });
        msgBox.SetOptions(options);
        msgBox.inMain.OnEndEditAsObservable()
            .Subscribe(str =>
            {
                if (int.TryParse(str, out _))
                {
                    msgBox.imgWarning.gameObject.SetActive(false);
                }
                else
                {
                    msgBox.imgWarning.gameObject.SetActive(true);
                }
                onValueChange?.Invoke(msgBox);
            });
        msgBox.tglUseCustomValue.OnValueChangedAsObservable()
            .Subscribe(isOn =>
            {
                msgBox.UseCustomNumber = isOn;
                onValueChange?.Invoke(msgBox);
            });
        msgBox.txtTitle.text = title;
        msgBox.txtContent.text = content;
        msgBox.txtOk.text = okText;
        msgBox.btnOk.OnClickAsObservable().Subscribe(_ =>
        {
            UIMgr.Instance.DestroyPanel<UICreateSeidBoxPanel>();
            onOk?.Invoke(msgBox);
        });
        msgBox.txtCancel.text = cancelText;
        msgBox.btnCancel.OnClickAsObservable().Subscribe(_ =>
        {
            UIMgr.Instance.DestroyPanel<UICreateSeidBoxPanel>();
            onCancel?.Invoke();
        });
        msgBox.UseCustomNumber = false;
        msgBox.Show();
        msgBox.ddlOption.value = 0;
        onValueChange?.Invoke(msgBox);
    }
}