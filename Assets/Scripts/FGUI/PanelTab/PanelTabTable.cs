using System;
using System.Collections.Generic;
using FairyGUI;
using UIPkg.Main;

public abstract class PanelTabTable : PanelTab
{
    public List<TableInfo> TableInfos { get; } = new List<TableInfo>();
    public UIComTableList TableList { get; set; }
    public int CurInspectIndex { get; set; }

    public override void OnAdd()
    {
        TableList = UIComTableList.CreateInstance();
        TableList.BindTable(TableInfos,GetData,GetDataCount,
            TableItemRenderer,OnClickTableItem);
        Content = TableList;
        CurInspectIndex = -1;
    }

    public override void OnOpen()
    {
        RefreshTable();
        InspectItem(CurInspectIndex);
    }

    public override void OnRemove()
    {
        Content.Dispose();
        TableInfos.Clear();
    }

    public void AddTableHeader(TableInfo info)
    {
        TableInfos.Add(info);
    }

    public void RefreshTable()
    {
        TableList.Refresh();
    }

    public void RefreshCurrentRow()
    {
        if(CurInspectIndex < 0)
            return;

        TableList.RefreshRowAt(CurInspectIndex);
    }
    
    private void TableItemRenderer(int index, UIComTableRow row)
    {
        if (index != TableList.m_list.selectedIndex)
        {
            row.GetController("button").selectedIndex = 0;
        }
    }

    private void OnClickTableItem(EventContext context)
    {
        if(!context.inputEvent.isDoubleClick)
            return;

        CurInspectIndex = TableList.m_list.selectedIndex;
        InspectItem(CurInspectIndex);
    }

    private void InspectItem(int index)
    {
        var data = index >= 0 ? GetData(index) : null;
        OnInspectItem(data);
    }

    protected abstract void OnInspectItem(object data);

    protected abstract object GetData(int index);

    protected abstract int GetDataCount();
}