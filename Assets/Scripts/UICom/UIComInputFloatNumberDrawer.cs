using System;
using UniRx;

public partial class UIComInputFloatNumberDrawer
{
    public string Title
    {
        get => txtTitle.text;
        set => txtTitle.text = value;
    }

    public string Content
    {
        get => inMain.text;
        set => inMain.SetTextWithoutNotify(value);
    }

    public Action<float> EndEdit;

    protected override void OnInit()
    {
        inMain.OnEndEditAsObservable()
            .Subscribe(str =>
            {
                if(float.TryParse(str,out var num))
                {
                    EndEdit?.Invoke(num);
                    imgWarning.gameObject.SetActive(false);
                }
                else
                {
                    imgWarning.gameObject.SetActive(true);
                }
            });
        
        imgWarning.gameObject.SetActive(false);
    }
        
}