using System;
using System.Collections.Generic;
using FairyGUI;
using UIPkg.Main;

public class WindowSelectorDialog : WindowDialogBase
{
    private UIWinSelectorDialog _dialog;
    private string _title;
    private List<TableInfo> _tableInfos;
    private List<int> _curIds = new List<int>();
    private List<int> _selection = new List<int>();
    private List<IModData> _dataList;
    private bool _allowMulti;
    private Action<List<int>> _onConfirm;
    private Action _onCancel;
    private bool _allowEmpty;


    public static void CreateDialog(string title,List<TableInfo> tableInfos,IEnumerable<int> curIds,bool allowEmpty,
        List<IModData> dataList,bool allowMulti, Action<List<int>> onConfirm = null, Action onCancel = null)
    {
        var window = new WindowSelectorDialog();
        window._title = title;
        window._tableInfos = tableInfos;
        window._onConfirm = onConfirm;
        window._onCancel = onCancel;
        window._allowEmpty = allowEmpty;
        window._allowMulti = allowMulti;
        window._curIds.AddRange(curIds);
        window._dataList = dataList;
        window.modal = true;
        
        window.Show();
    }
    
    protected override void OnInit()
    {
        _dialog = UIWinSelectorDialog.CreateInstance();
        contentPane = _dialog;

        _dialog.m_frame.title = _title;
        closeButton.onClick.Set(Cancel);
        _dialog.m_closeButton.onClick.Set(Cancel);
        _dialog.m_btnOk.onClick.Set(Confirm);

        _dialog.m_table.BindTable(_tableInfos, i => _dataList[i], () => _dataList.Count,
            OnItemRenderer, _ => OnClickListItem());

        if (_allowMulti)
        {
            _dialog.m_table.m_list.selectionMode = ListSelectionMode.Multiple_SingleClick;
        }
        else
        {
            _dialog.m_table.m_list.selectionMode = ListSelectionMode.Single;
            
        }
        
        foreach (var curId in _curIds)
        {
            int idIndex = -1;
            for (var index = 0; index < _dataList.Count; index++)
            {
                if (_dataList[index].ID != curId)
                    continue;
                idIndex = index;
                break;
            }

            if (idIndex >= 0)
                _dialog.m_table.m_list.AddSelection(idIndex, false);
        }
        _dialog.m_table.m_list.GetSelection(_selection);

        RefreshConfirm();
        RefreshTipText();
    }

    private void OnClickListItem()
    {
        _selection.Clear();
        _dialog.m_table.m_list.GetSelection(_selection);
        RefreshTipText();
        RefreshConfirm();
    }

    private void RefreshTipText()
    {
        if (_selection.Count == 0)
        {
            _dialog.m_txtTips.text = "未选择列表项。";
        }
        else
        {
            _dialog.m_txtTips.text = $"当前已选择 {_selection.Count}项。";
        }
    }

    private void OnItemRenderer(int index,UIComTableRow row)
    {
        if (!_selection.Contains(index))
        {
            row.GetController("button").selectedIndex = 0;
        }
    }

    private void RefreshConfirm()
    {
        if (!_allowEmpty)
        {
            _dialog.m_btnOk.enabled = _selection.Count > 0;
        }
        else
        {
            _dialog.m_btnOk.enabled = true;
        }
    }
    
    private void Confirm()
    {
        _curIds.Clear();
        foreach (var idIndex in _selection)
        {
            _curIds.Add(_dataList[idIndex].ID);
        }
        _curIds.Sort();

        _onConfirm?.Invoke(_curIds);
        Hide();
    }

    private void Cancel()
    {
        _onCancel?.Invoke();
        Hide();
    }
}