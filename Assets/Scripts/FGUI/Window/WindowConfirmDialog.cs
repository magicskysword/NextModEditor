using System;
using FairyGUI;
using UIPkg.Main;

public class WindowConfirmDialog : WindowDialogBase
{
    private string _title;
    private string _showText;
    private Action _onConfirm;
    private Action _onCancel;

    public static void CreateDialog(string title,string text, Action onConfirm = null,Action onCancel = null)
    {
        var window = new WindowConfirmDialog();
        window._showText = text;
        window._onConfirm = onConfirm;
        window._onCancel = onCancel;
        window._title = title;
        window.modal = true;
        
        window.Show();
    }
    
    protected override void OnInit()
    {
        var dialog = UIWinConfirmDialog.CreateInstance();
        contentPane = dialog;

        dialog.m_frame.title = _title;
        dialog.m_text.text = _showText;
        dialog.m_closeButton.onClick.Set(Cancel);
        dialog.m_btnOk.onClick.Set(Confirm);
    }

    private void Confirm()
    {
        _onConfirm?.Invoke();
        Hide();
    }

    private void Cancel()
    {
        _onCancel?.Invoke();
        Hide();
    }
}