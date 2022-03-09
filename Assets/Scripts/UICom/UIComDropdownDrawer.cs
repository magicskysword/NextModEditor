using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine.UI;

public partial class UIComDropdownDrawer
{
    public string Title
    {
        get => txtTitle.text;
        set => txtTitle.text = value;
    }

    public Action<int> ValueChange;

    protected override void OnInit()
    {
        ddlMain.OnValueChangedAsObservable()
            .Subscribe(index => ValueChange?.Invoke(index));
    }

    public void SetOptions(List<string> options)
    {
        ddlMain.ClearOptions();
        ddlMain.AddOptions(options);
    }

    public void Select(int index)
    {
        if(index >= 0)
            ddlMain.SetValueWithoutNotify(index);
        else
            ddlMain.SetValueWithoutNotify(0);
    }
}