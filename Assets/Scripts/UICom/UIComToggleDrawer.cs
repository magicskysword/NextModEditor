using System;
using UniRx;

public partial class UIComToggleDrawer
{
    public Action<bool> OnValueChange;

    public string Title
    {
        get => txtTitle.text;
        set => txtTitle.text = value;
    }

    public bool IsOn
    {
        get => tglMain.isOn;
        set => tglMain.SetIsOnWithoutNotify(value);
    }

    protected override void OnInit()
    {
        tglMain.OnValueChangedAsObservable()
            .Subscribe(isOn =>
            {
                OnValueChange?.Invoke(isOn);
            });
    }
}