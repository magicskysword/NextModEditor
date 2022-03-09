using System;
using UniRx;

public partial class UISelectBoxPanel
{
    public Action OnFunc1;
    public Action OnFunc2;
    public Action OnCancel;

    protected override void OnInit()
    {
        btnFunc1.OnClickAsObservable()
            .Subscribe(_ =>
            {
                UIMgr.Instance.DestroyPanel<UISelectBoxPanel>();
                OnFunc1?.Invoke();
            });
        btnFunc2.OnClickAsObservable()
            .Subscribe(_ =>
            {
                UIMgr.Instance.DestroyPanel<UISelectBoxPanel>();
                OnFunc2?.Invoke();
            });
        btnCancel.OnClickAsObservable()
            .Subscribe(_ => { OnCloseUI(); });
    }

    public void OnCloseUI()
    {
        UIMgr.Instance.DestroyPanel<UISelectBoxPanel>();
        OnCancel?.Invoke();
    }

    public static void ShowMessage(string title, string content,
        string func1Text = "确定", string func2Text = "确定", string cancelText = "取消",
        Action onFunc1 = null, Action onFunc2 = null, Action onCancel = null)
    {
        var msgBox = UIMgr.Instance.GetPanel<UISelectBoxPanel>(UILayer.Model);
        msgBox.txtTitle.text = title;
        msgBox.txtContent.text = content;
        msgBox.txtFunc1.text = func1Text;
        msgBox.txtFunc2.text = func2Text;
        msgBox.txtCancel.text = cancelText;
        msgBox.OnFunc1 = onFunc1;
        msgBox.OnFunc2 = onFunc2;
        msgBox.OnCancel = onCancel;
        msgBox.Show();
    }
}