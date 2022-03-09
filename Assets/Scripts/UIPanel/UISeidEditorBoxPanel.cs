using System;
using System.Linq;
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
                inputNumberDrawer.EndEdit = i =>
                {
                    sInt.Value = i; 
                    RefreshUI();
                };

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
                    RefreshUI();
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
            switch (drawerId)
            {
                case "BuffDrawer":
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
                    };
                    OnBuffDrawerRefresh();
                    break;
                }
                case "SeidDrawer":
                {
                    var seidDrawer = AddEditorDrawer<UIComTextWithContentDrawer>();
                    seidDrawer.Title = "功能描述";
                    
                    void OnSeidDrawerRefresh()
                    {
                        if (ModMgr.Instance.BuffSeidMetas.TryGetValue(sInt.Value,out var seidMeta))
                        {
                            seidDrawer.Content = $"{sInt.Value} 【{seidMeta.Name}】";
                        }
                        else
                        {
                            seidDrawer.Content = $"{sInt.Value} 【？】";
                        }
                    }

                    inputNumberDrawer.EndEdit += i =>
                    {
                        OnSeidDrawerRefresh();
                    };
                    OnSeidDrawerRefresh();
                    break;
                }
                case "BuffTypeDrawer":
                {
                    var seidDrawer = AddEditorDrawer<UIComDropdownDrawer>();
                    seidDrawer.Title = "Buff类型选择器";
                    
                    void OnSeidDrawerRefresh()
                    {
                        var typeValue = sInt.Value;
                        var typeList = ModMgr.Instance.BuffDataBuffTypes
                            .Select(type => $"{type.TypeID} {type.TypeName}").ToList();
                        seidDrawer.SetOptions(typeList);
                        var typeIndex = ModMgr.Instance.BuffDataBuffTypes.FindIndex(type => type.TypeID == typeValue);
                        seidDrawer.Select(typeIndex);
                        seidDrawer.ValueChange = index =>
                        {
                            var typeId = ModMgr.Instance.BuffDataBuffTypes[index].TypeID;
                            inputNumberDrawer.Content = typeId.ToString();
                        };
                    }

                    inputNumberDrawer.EndEdit += i =>
                    {
                        OnSeidDrawerRefresh();
                    };
                    OnSeidDrawerRefresh();
                    break;
                }
            }
        }
        
        
    }

    private void CreateIntArraySpecialDrawer(UIComInputTextDrawer inputTextDrawer, ModSeidProperty seidProperty,
        ModSIntArray sIntArray)
    {
        foreach (var drawerId in seidProperty.SpecialDrawer)
        {
            switch (drawerId)
            {
                case "BuffArrayDrawer":
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
                            }
                            buffDrawer.Content = sb.ToString();
                        }
                        else
                        {
                            buffDrawer.gameObject.SetActive(false);
                        }
                    }

                    inputTextDrawer.EndEdit += i =>
                    {
                        OnBuffDrawerRefresh();
                    };
                    OnBuffDrawerRefresh();
                    break;
                }
                case "SeidArrayDrawer":
                {
                    var seidDrawer = AddEditorDrawer<UIComTextWithContentDrawer>();
                    seidDrawer.Title = "功能描述";

                    void OnSeidDrawerRefresh()
                    {
                        if (sIntArray.Value.Count > 0)
                        {
                            seidDrawer.gameObject.SetActive(true);
                            var sb = new StringBuilder();
                            foreach (var seidId in sIntArray.Value)
                            {
                                if (ModMgr.Instance.BuffSeidMetas.TryGetValue(seidId,out var seidMeta))
                                {
                                    sb.Append($"{seidId} 【{seidMeta.Name}】");
                                }
                                else
                                {
                                    sb.Append($"{seidId} 【？】");
                                }
                            }
                            seidDrawer.Content = sb.ToString();
                        }
                        else
                        {
                            seidDrawer.gameObject.SetActive(false);
                        }
                    }

                    inputTextDrawer.EndEdit += i =>
                    {
                        OnSeidDrawerRefresh();
                    };
                    OnSeidDrawerRefresh();
                    break;
                }
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