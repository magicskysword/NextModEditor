using System;
using UniRx;

public partial class UIComInputNumberDrawer
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

    public Action<int> EndEdit;

    protected override void OnInit()
    {
        inMain.OnEndEditAsObservable()
            .Subscribe(str =>
            {
                if(int.TryParse(str,out var num))
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