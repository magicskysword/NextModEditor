using UniRx;

public partial class UIAboutPanel : IUIClose
{
    protected override void OnInit()
    {
        btnClose.OnClickAsObservable()
            .Subscribe(_ =>
            {
                OnCloseUI();
            });
    }

    public void OnCloseUI()
    {
        UIMgr.Instance.DestroyPanel<UIAboutPanel>();
    }
}