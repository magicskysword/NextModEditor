using System;
using System.Collections.Generic;
using System.Linq;

public class UIComInputIdDrawer : UIComInputNumberDrawer
{
    private Func<IEnumerable<IModData>> dataListGetter;

    public IEnumerable<IModData> DataList => dataListGetter.Invoke();

    public void BindEditor(IModDataEditor dataEditor,
        Func<IEnumerable<IModData>> dataListGetter,
        Func<IModData,string> dataTitleGetter,
        Action<IModData,int> onChangeId,
        Action<IModData,IModData> onSwiftId,
        Action onCancel)
    {
        this.dataListGetter = dataListGetter;
        
        EndEdit = num =>
        {
            var selectedItem = dataEditor.SelectModData;
            if(num == selectedItem.ID)
                return;
            
            var otherData = DataList.FirstOrDefault(data => data.ID == num && data != selectedItem);

            if (otherData != null)
            {
                UIConfirmBoxPanel.ShowMessage("提示",
                    $"已经存在ID为 {num} 的数据，是否交换 {dataTitleGetter.Invoke(selectedItem)} 与 {dataTitleGetter.Invoke(otherData)} ID？",
                    onOk: () =>
                    {
                        onSwiftId?.Invoke(selectedItem, otherData);
                    },
                    onCancel: () =>
                    {
                        onCancel?.Invoke();
                    });
            }
            else
            {
                UIConfirmBoxPanel.ShowMessage(
                    "提示",
                    $"即将把 {dataTitleGetter.Invoke(selectedItem)} 的ID修改为 {num}，是否继续？",
                    onOk: () =>
                    {
                        onChangeId?.Invoke(selectedItem, num);
                    },
                    onCancel: () =>
                    {
                        onCancel?.Invoke();
                    });
            }
        };
    }
}