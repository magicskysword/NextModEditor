using System;

public partial class UIMessageScrollBoxPanel
{
    public static void ShowMessage(string title, string content, string okText = "确定", Action callback = null)
    {
        var msgBox = UIMgr.Instance.GetPanel<UIMessageScrollBoxPanel>(UILayer.Model);
        msgBox.txtTitle.text = title;
        msgBox.txtContent.text = content;
        msgBox.txtOk.text = okText;
        msgBox.btnOk.onClick.RemoveAllListeners();
        msgBox.btnOk.onClick.AddListener(() =>
        {
            UIMgr.Instance.DestroyPanel<UIMessageScrollBoxPanel>();
            callback?.Invoke();
        });
        msgBox.Show();
    }
}