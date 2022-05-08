using System;
using UniRx;

public partial class UIMessageBoxPanel
{
    public static void ShowMessage(string title, string content, string okText = "确定", Action callback = null)
    {
        var msgBox = UIMgr.Instance.GetPanel<UIMessageBoxPanel>(UILayer.Model);
        msgBox.txtTitle.text = title;
        msgBox.txtContent.text = content;
        msgBox.txtOk.text = okText;
        msgBox.btnOk.onClick.RemoveAllListeners();
        msgBox.btnOk.onClick.AddListener(() =>
        {
            UIMgr.Instance.DestroyPanel<UIMessageBoxPanel>();
            callback?.Invoke();
        });
        msgBox.Show();
    }
}