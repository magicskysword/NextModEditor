using System;
using UniRx;

public partial class UIComInputTextDrawer
{
    public string Title
    {
        get => txtTitle.text;
        set => txtTitle.text = value;
    }

    public string Content
    {
        get => inMain.text;
        set => inMain.text = value;
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