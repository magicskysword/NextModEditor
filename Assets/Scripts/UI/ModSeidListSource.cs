using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VirtualList;

public class ModSeidListSource : IListSource,IListSourceSelected
{
    public List<ModSeidMeta> SeidList { get; set; }
    public int SelectedIndex { get; set; }
    
    public event Action<ModSeidMeta> SelectData;
    public event Action<UIComTglListItem, ModSeidMeta> RendererItem;

    private List<UIComTglListItem> Items { get; set; } = new List<UIComTglListItem>();

    public int Count => SeidList.Count;
    public void SetItem(GameObject view, int index)
    {
        var item = view.GetComponent<UIComTglListItem>();
        if (item == null)
        {
            item = view.AddComponent<UIComTglListItem>();
            item.Init();
            item.SelectItem = OnSelectData;
            item.UnselectItem = OnUnselectData;
            Items.Add(item);
        }
        
        item.BindIndex = index;
        
        if (index == SelectedIndex)
        {
            item.tglTab.SetIsOnWithoutNotify(true);
        }
        else
        {
            item.tglTab.SetIsOnWithoutNotify(false);
        }
        var data = SeidList[index];
        OnRendererItem(item, data);
    }

    public void SwitchOffAllToggle(bool isOn)
    {
        foreach (var listItem in Items)
        {
            listItem.tglTab.SetIsOnWithoutNotify(isOn);
        }
    }

    private void OnUnselectData(UIComTglListItem item, int index)
    {
        if(SelectedIndex == index)
            item.tglTab.SetIsOnWithoutNotify(true);
    }

    private void OnSelectData(UIComTglListItem item,int index)
    {
        if(index >= 0 && index < SeidList.Count)
        {
            SwitchOffAllToggle(false);
            item.tglTab.SetIsOnWithoutNotify(true);
            var data = SeidList[index];
            SelectedIndex = index;
            SelectData?.Invoke(data);
        }
    }

    private void OnRendererItem(UIComTglListItem item, ModSeidMeta data)
    {
        RendererItem?.Invoke(item, data);
    }
}

