using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;

public partial class UIModCreateSeidBoxPanel : IUIClose
{
    private Action<UIModCreateSeidBoxPanel> OnValueChanged;
    private Action<UIModCreateSeidBoxPanel> OnOk;
    private Action<UIModCreateSeidBoxPanel> OnCancel;
    private ModSeidMeta _selectedItem;
    private List<ModSeidMeta> SeidMetas { get; set; }
    private ModSeidListSource SeidListSource { get; set; }

    public ModSeidMeta SelectedItem
    {
        get => _selectedItem;
        private set
        {
            _selectedItem = value;
            OnValueChanged?.Invoke(this);
        }
    }

    public bool UseCustomNumber
    {
        get => tglUseCustomValue.isOn;
        set
        {
            tglUseCustomValue.SetIsOnWithoutNotify(value);
            if (value)
            {
                // Custom Number
                goCustomRoot.gameObject.SetActive(true);
                goListRoot.gameObject.SetActive(false);
            }
            else
            {
                // Selection List
                goCustomRoot.gameObject.SetActive(false);
                goListRoot.gameObject.SetActive(true);
            }
        }
    }
    public int CustomNumber => int.TryParse(inCustomId.text, out var num) ? num : -1;
    public int SelectedId => SelectedItem?.ID ?? -1;

    protected override void OnInit()
    {
        inSearch.OnValueChangedAsObservable().Subscribe(SetFilter);
        inCustomId.OnEndEditAsObservable()
            .Subscribe(str =>
            {
                if (int.TryParse(str, out _))
                {
                    imgWarning.gameObject.SetActive(false);
                }
                else
                {
                    imgWarning.gameObject.SetActive(true);
                }
                OnValueChanged?.Invoke(this);
            });
        tglUseCustomValue.OnValueChangedAsObservable()
            .Subscribe(isOn =>
            {
                UseCustomNumber = isOn;
                OnValueChanged?.Invoke(this);
            });
        btnOk.OnClickAsObservable().Subscribe(_ =>
        {
            OnOk?.Invoke(this);
            UIMgr.Instance.DestroyPanel<UIModCreateSeidBoxPanel>();
            
        });
        btnCancel.OnClickAsObservable().Subscribe(_ => { OnCloseUI(); });
        
        inCustomId.text = "0";
        imgWarning.gameObject.SetActive(false);
        UseCustomNumber = false;
    }

    public void OnCloseUI()
    {
        OnCancel?.Invoke(this);
        UIMgr.Instance.DestroyPanel<UIModCreateSeidBoxPanel>();
    }

    private void InitOptions()
    {
        SeidListSource = new ModSeidListSource();

        SeidListSource.RendererItem += (item,data) => item.txtTab.text = $"{data.ID} {data.Name}";;
        SeidListSource.SelectData += item => SelectedItem = item;
        
        SetFilter(null);
    }
    
    public void SetFilter(string filter)
    {
        if(SeidListSource == null)
            return;

        if(string.IsNullOrEmpty(filter))
        {
            SeidListSource.SeidList = SeidMetas;
        }
        else
        {
            SeidListSource.SeidList = SeidMetas.Where(seid =>
                seid.ID.ToString().Contains(filter) || 
                seid.Name.Contains(filter) ||
                seid.Desc.Contains(filter)).ToList();
        }
        SelectedItem = SeidListSource.SeidList.FirstOrDefault();
        SeidListSource.SelectedIndex = SelectedItem != null ? 0 : -1;
        vlstItems.SetSource(SeidListSource);
        lstTabs.verticalNormalizedPosition = 1f;
        vlstItems.Invalidate();
    }

    public static void ShowMessage(string title, string content, List<ModSeidMeta> options,
        string okText = "确定", string cancelText = "取消", 
        Action<UIModCreateSeidBoxPanel> onValueChanged = null,
        Action<UIModCreateSeidBoxPanel> onOk = null, 
        Action<UIModCreateSeidBoxPanel> onCancel = null)
    {
        var msgBox = UIMgr.Instance.GetPanel<UIModCreateSeidBoxPanel>(UILayer.Model);
        msgBox.OnValueChanged = onValueChanged;
        msgBox.OnOk = onOk;
        msgBox.OnCancel = onCancel;
        msgBox.SeidMetas = options;
        msgBox.InitOptions();
        msgBox.txtTitle.text = title;
        msgBox.txtContent.text = content;
        msgBox.txtOk.text = okText;
        msgBox.txtCancel.text = cancelText;
        msgBox.UseCustomNumber = false;
        msgBox.Show();
        msgBox.inSearch.Select();
    }
}