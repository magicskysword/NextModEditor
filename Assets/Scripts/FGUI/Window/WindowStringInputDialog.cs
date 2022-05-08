using System;
using FairyGUI;
using UIPkg.Main;

public class WindowStringInputDialog : WindowDialogBase
{
    private string _title;
    private string _defaultText;
    private Action<string> _onConfirm;
    private Action<string> _onCancel;

    public static void CreateDialog(string title,string text, Action<string> onConfirm = null,Action<string> onCancel = null)
    {
        var window = new WindowStringInputDialog();
        window._defaultText = text;
        window._onConfirm = onConfirm;
        window._onCancel = onCancel;
        window._title = title;
        window.modal = true;
        
        window.Show();
    }
    
    protected override void OnInit()
    {
        var dialog = UIWinStringInputDialog.CreateInstance();
        contentPane = dialog;

        dialog.m_frame.title = _title;
        dialog.m_inContent.text = _defaultText;
        closeButton.onClick.Set(() => Cancel(dialog.m_inContent.text));
        dialog.m_closeButton.onClick.Set(() => Cancel(dialog.m_inContent.text));
        dialog.m_btnOk.onClick.Set(() => Confirm(dialog.m_inContent.text));
    }

    private void Confirm(string getStr)
    {
        _onConfirm?.Invoke(getStr);
        Hide();
    }

    private void Cancel(string getStr)
    {
        _onCancel?.Invoke(getStr);
        Hide();
    }
}