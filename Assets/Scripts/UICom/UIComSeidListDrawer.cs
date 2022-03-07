using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;

public class UIComSeidListDrawer : UIComListDrawer
{
    private Func<Dictionary<int, ModSeidMeta>> seidMetaGetter;
    private Func<IModSeidDataGroup> seidGroupGetter;
    private Func<List<int>> seidListGetter;

    public Dictionary<int, ModSeidMeta> SeidMetas => seidMetaGetter.Invoke();
    public IModSeidDataGroup SeidDataGroup => seidGroupGetter.Invoke();
    public List<int> SeidList => seidListGetter.Invoke();

    public void BindSeid(IModDataEditor modDataEditor,
        Func<Dictionary<int, ModSeidMeta>> seidMetaGetter,
        Func<IModSeidDataGroup> seidGroupGetter,
        Func<List<int>> seidListGetter)
    {
        this.seidMetaGetter = seidMetaGetter;
        this.seidGroupGetter = seidGroupGetter;
        this.seidListGetter = seidListGetter;

        var commonEditor = modDataEditor.CommonEditor;
        OnBtnAddClick = () =>
        {
            var selectedItem = modDataEditor.SelectModData;
            var seidMetaList =
                SeidMetas.Values.OrderBy(item => item.ID).ToList();
            UIModCreateSeidBoxPanel.ShowMessage(
                "新建",
                "请选择要创建的Seid（不可与已有Seid重复）",
                seidMetaList,
                onValueChanged: panel =>
                {
                    int curId;
                    if (panel.UseCustomNumber)
                    {
                        curId = panel.CustomNumber;
                    }
                    else
                    {
                        curId = panel.SelectedId;
                    }

                    if (curId < 0 || SeidList.Contains(curId))
                    {
                        panel.btnOk.interactable = false;
                    }
                    else
                    {
                        panel.btnOk.interactable = true;
                    }
                },
                onOk: panel =>
                {
                    ModSeidMeta seidMeta;
                    int seidId;
                    if (panel.UseCustomNumber)
                    {
                        seidId = panel.CustomNumber;
                        seidMeta = seidMetaList.FirstOrDefault(meta => meta.ID == seidId);
                    }
                    else
                    {
                        seidMeta = panel.SelectedItem;
                        seidId = seidMeta.ID;
                    }

                    if (seidMeta != null && seidMeta.Properties.Count > 0)
                    {
                        SeidDataGroup.GetOrCreateSeid(selectedItem.ID, seidMeta.ID);
                    }

                    SeidList.Add(seidId);
                    SeidList.Sort();
                    commonEditor.RefreshItem(selectedItem);
                });
        };
        OnBtnEditClick = item =>
        {
            var selectedItem = modDataEditor.SelectModData;
            var seidItem = (UIComSeidListItem)item;
            if (SeidMetas.TryGetValue(seidItem.SeidID, out var seidMeta))
            {
                var seidData = SeidDataGroup.GetOrCreateSeid(selectedItem.ID, seidItem.SeidID);
                UISeidEditorBoxPanel.ShowEditor(seidData, seidMeta, $"Seid {seidMeta.ID} {seidMeta.Name} 编辑");
            }
        };
        OnBtnRemoveClick = item =>
        {
            var seidItem = (UIComSeidListItem)item;
            UIConfirmBoxPanel.ShowMessage(
                "警告",
                $"即将删除 Seid {seidItem.SeidTitle} ，该操作不可恢复，是否继续？",
                onOk: () =>
                {
                    var selectedItem = modDataEditor.SelectModData;
                    SeidDataGroup.RemoveSeid(selectedItem.ID, seidItem.SeidID);
                    SeidList.Remove(seidItem.SeidID);
                    commonEditor.RefreshItem(selectedItem);
                });
        };
        OnListItemChangeOrder = (from, to) =>
        {
            var seid = SeidList[from];
            SeidList.RemoveAt(from);
            SeidList.Insert(to,seid);
        };
    }

    public void Refresh(Func<ModSeidMeta,string> titleGetter)
    {
        Clear();
        foreach (var seidId in SeidList)
        {
            var item = AddItem<UIComSeidListItem>();
            item.SeidID = seidId;
            if(SeidMetas.TryGetValue(seidId,out var seidMeta))
            {
                item.SeidTitle = titleGetter.Invoke(seidMeta);
                item.CanEdit = seidMeta.Properties.Count > 0;
            }
            else
            {
                item.SeidTitle = $"{seidId}";
                item.CanEdit = false;
            }
        }
        Observable.EveryUpdate().First().Subscribe(_ =>
        {
            rolstMain.GetRectTransform().ForceRebuildLayoutImmediate();
        });
    }
}