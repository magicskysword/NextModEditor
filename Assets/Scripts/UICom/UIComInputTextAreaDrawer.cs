using System;
using UniRx;

public partial class UIComInputTextAreaDrawer
{
    public string Title
    {
        get => txtTitle.text;
        set => txtTitle.text = value;
    }

    public string Content
    {
        get => inMain.text;
        set
        {
            inMain.SetTextWithoutNotify(value);
            Observable.EveryUpdate().First().Subscribe(_ =>
            {
                inMain.verticalScrollbar.value = 0;
            });
        }
    }

    public Action<string> EndEdit;

    protected override void OnInit()
    {
        inMain.OnEndEditAsObservable()
            .Subscribe(str =>
            {
                EndEdit?.Invoke(str);
            });
        imgWarning.gameObject.SetActive(false);
    }

    public void ShowWarning(string tip)
    {
        imgWarning.gameObject.SetActive(true);
    }

    public void HideWarning()
    {
        imgWarning.gameObject.SetActive(false);
    }
}