using System;
using UniRx;

public partial class UIConfirmBoxPanel : IUIClose
{
    public Action OnOk;
    public Action OnCancel;
    
    protected override void OnInit()
    {
        btnOk.OnClickAsObservable()
            .Subscribe(_ =>
            {
                UIMgr.Instance.DestroyPanel<UIConfirmBoxPanel>();
                OnOk?.Invoke();
            });
        btnCancel.OnClickAsObservable()
            .Subscribe(_ =>
            {
                OnCloseUI();
            });
    }
    
    public void OnCloseUI()
    {
        UIMgr.Instance.DestroyPanel<UIConfirmBoxPanel>();
        OnCancel?.Invoke();
    }

    public static void ShowMessage(string title, string content, 
        string okText = "确定", string cancelText = "取消", 
        Action onOk = null, Action onCancel = null)
    {
        var msgBox = UIMgr.Instance.GetPanel<UIConfirmBoxPanel>(UILayer.Model);
        msgBox.txtTitle.text = title;
        msgBox.txtContent.text = content;
        msgBox.txtOk.text = okText;
        msgBox.txtCancel.text = cancelText;
        msgBox.OnOk = onOk;
        msgBox.OnCancel = onCancel;
        msgBox.Show();
    }
}