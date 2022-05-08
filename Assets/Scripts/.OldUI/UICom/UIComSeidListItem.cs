using UnityEngine.UI;

public partial class UIComSeidListItem : IListDrawerItem
{
    public Button BtnRemove => btnRemove;
    public Button BtnEdit => btnEdit;
    
    public int SeidID { get; set; }

    public string SeidTitle
    {
        get => txtTitle.text;
        set => txtTitle.text = value;
    }

    public bool CanEdit
    {
        get => btnEdit.gameObject.activeSelf;
        set => btnEdit.gameObject.SetActive(value);
    }

    protected override void OnInit()
    {
        btnWarning.gameObject.SetActive(false);
    }
}