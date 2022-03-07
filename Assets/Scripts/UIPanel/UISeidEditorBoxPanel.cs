using System;
using System.Text;
using UniRx;

public partial class UISeidEditorBoxPanel : IUIClose
{
    public Action OnCloseCallback;
    public ModSeidData SeidData { get; set; }
    public ModSeidMeta SeidMeta { get; set; }
    
    
    public T AddEditorDrawer<T>() where T : UIComBase
    {
        var drawer = UIMgr.Instance.CreateCom<T>(goDrawerRoot);
        return drawer;
    }

    public void BindSeid(ModSeidData seidData, ModSeidMeta seidMeta)
    {
        SeidData = seidData;
        SeidMeta = seidMeta;
        var textDrawer = AddEditorDrawer<UIComTextDrawer>();
        textDrawer.Title = SeidMeta.Desc;
        foreach (var seidProperty in SeidMeta.Properties)
        {
            if (seidProperty.Type == ModSeidPropertyType.Int)
            {
                var inputNumberDrawer = AddEditorDrawer<UIComInputNumberDrawer>();
                inputNumberDrawer.Title = seidProperty.Desc;
                var sInt = seidData.GetToken<ModSInt>(seidProperty.ID);
                inputNumberDrawer.Content = sInt.Value.ToString();
                inputNumberDrawer.EndEdit = i => { sInt.Value = i; };

                CreateIntSpecialDrawer(inputNumberDrawer, seidProperty, sInt);
            }
            else if (seidProperty.Type == ModSeidPropertyType.IntArray)
            {
                var inputTextDrawer = AddEditorDrawer<UIComInputTextDrawer>();
                inputTextDrawer.Title = seidProperty.Desc;
                var sIntArray = seidData.GetToken<ModSIntArray>(seidProperty.ID);
                inputTextDrawer.Content = sIntArray.Value.ToFormatString();
                inputTextDrawer.EndEdit = str =>
                {
                    if (str.TryFormatToListInt(out var list))
                    {
                        sIntArray.Value = list;
                        inputTextDrawer.HideWarning();
                        inputTextDrawer.Content = sIntArray.Value.ToFormatString();
                    }
                    else
                    {
                        inputTextDrawer.ShowWarning("");
                    }
                };

                CreateIntArraySpecialDrawer(inputTextDrawer, seidProperty, sIntArray);
            }
        }

        RefreshUI();
    }

    private void CreateIntSpecialDrawer(UIComInputNumberDrawer inputNumberDrawer,ModSeidProperty seidProperty,ModSInt sInt)
    {
        foreach (var drawerId in seidProperty.SpecialDrawer)
        {
            if (drawerId == "BuffDrawer")
            {
                var buffDrawer = AddEditorDrawer<UIComTextWithContentDrawer>();
                buffDrawer.Title = "Buff描述";

                void OnBuffDrawerRefresh()
                {
                    var buffData = ModMgr.Instance.CurProject.FindBuff(sInt.Value);
                    if (buffData != null)
                    {
                        buffDrawer.Content = $"【{sInt.Value} {buffData.Name}】{buffData.Desc}";
                    }
                    else
                    {
                        buffDrawer.Content = $"【{sInt.Value}  ？】";
                    }
                }

                inputNumberDrawer.EndEdit += i =>
                {
                    OnBuffDrawerRefresh();
                    RefreshUI();
                };
                OnBuffDrawerRefresh();
            }
        }
        
        
    }

    private void CreateIntArraySpecialDrawer(UIComInputTextDrawer inputTextDrawer, ModSeidProperty seidProperty,
        ModSIntArray sIntArray)
    {
        foreach (var drawerId in seidProperty.SpecialDrawer)
        {
            if (drawerId == "BuffArrayDrawer")
            {
                var buffDrawer = AddEditorDrawer<UIComTextWithContentDrawer>();
                buffDrawer.Title = "Buff描述";

                void OnBuffDrawerRefresh()
                {
                    if (sIntArray.Value.Count > 0)
                    {
                        buffDrawer.gameObject.SetActive(true);
                        var sb = new StringBuilder();
                        foreach (var buffId in sIntArray.Value)
                        {
                            var buffData = ModMgr.Instance.CurProject.FindBuff(buffId);
                            if (buffData != null)
                            {
                                sb.Append($"【{buffId} {buffData.Name}】{buffData.Desc}\n");
                            }
                            else
                            {
                                sb.Append($"【{buffId} ？】\n");
                            }

                            buffDrawer.Content = sb.ToString();
                        }
                    }
                    else
                    {
                        buffDrawer.gameObject.SetActive(false);
                    }
                }

                inputTextDrawer.EndEdit += i =>
                {
                    OnBuffDrawerRefresh();
                    RefreshUI();
                };
                OnBuffDrawerRefresh();
            }
        }
    }
    
    public void RefreshUI()
    {
        Observable.EveryUpdate().First().Subscribe(_ =>
        {
            goDrawerRoot.ForceRebuildLayoutImmediate();
        });
    }
    
    public void OnCloseUI()
    {
        UIMgr.Instance.DestroyPanel<UISeidEditorBoxPanel>();
        OnCloseCallback?.Invoke();
    }

    public static void ShowEditor(ModSeidData seidData, ModSeidMeta seidMeta,string title, 
        string okText = "确定", Action callback = null)
    {
        var editor = UIMgr.Instance.GetPanel<UISeidEditorBoxPanel>(UILayer.Model);
        editor.OnCloseCallback = callback;
        editor.BindSeid(seidData,seidMeta);
        editor.txtTitle.text = title;
        editor.txtOk.text = okText;
        editor.btnOk.onClick.RemoveAllListeners();
        editor.btnOk.onClick.AddListener(() =>
        {
            editor.OnCloseUI();
        });
        editor.Show();
    }
}