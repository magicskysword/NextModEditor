using System;
using UniRx;

public partial class UIComTglListItem
{
    public int BindIndex { get; set; }
    public Action<UIComTglListItem,int> SelectItem;
    public Action<UIComTglListItem,int> UnselectItem;

    protected override void OnInit()
    {
        tglTab.OnValueChangedAsObservable()
            .Subscribe(isOn =>
            {
                if (isOn)
                    SelectItem?.Invoke(this, BindIndex);
                else 
                    UnselectItem?.Invoke(this, BindIndex);
            });
    }
}