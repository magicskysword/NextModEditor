using System;
using System.Globalization;
using System.Linq;
using System.Text;
using UniRx;

public partial class UIModSeidEditorBoxPanel : IUIClose
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
            else if (seidProperty.Type == ModSeidPropertyType.Float)
            {
                var inputFloatNumberDrawer = AddEditorDrawer<UIComInputFloatNumberDrawer>();
                inputFloatNumberDrawer.Title = seidProperty.Desc;
                var sFloat = seidData.GetToken<ModSFloat>(seidProperty.ID);
                inputFloatNumberDrawer.Content = sFloat.Value.ToString("F");
                inputFloatNumberDrawer.EndEdit = i =>
                {
                    sFloat.Value = i;
                    inputFloatNumberDrawer.Content = sFloat.Value.ToString("F");
                    RefreshUI();
                };
            }
            else if (seidProperty.Type == ModSeidPropertyType.String)
            {
                var inputTextDrawer = AddEditorDrawer<UIComInputTextDrawer>();
                inputTextDrawer.Title = seidProperty.Desc;
                var sString = seidData.GetToken<ModSString>(seidProperty.ID);
                inputTextDrawer.Content = sString.Value;
                inputTextDrawer.EndEdit = i =>
                {
                    sString.Value = i; 
                    RefreshUI();
                };

                CreateStringSpecialDrawer(inputTextDrawer, seidProperty, sString);
            }
        }

        RefreshUI();
    }

    private void CreateStringSpecialDrawer(UIComInputTextDrawer inputTextDrawer, ModSeidProperty seidProperty, ModSString sString)
    {
        foreach (var drawerId in seidProperty.SpecialDrawer)
        {
            switch (drawerId)
            {
                case "ComparisonOperatorTypeDrawer":
                {
                    var targetDrawer = AddEditorDrawer<UIComDropdownDrawer>();
                    targetDrawer.Title = "比较符";
                    
                    void OnDrawerRefresh()
                    {
                        var typeValue = sString.Value;
                        var typeList = ModMgr.Instance.ComparisonOperatorTypes
                            .Select(type => $"{type.ID} {type.Desc}").ToList();
                        targetDrawer.SetOptions(typeList);
                        var typeIndex = ModMgr.Instance.ComparisonOperatorTypes.FindIndex(type => type.ID == typeValue);
                        targetDrawer.Select(typeIndex);
                        targetDrawer.ValueChange = index =>
                        {
                            var typeId = ModMgr.Instance.ComparisonOperatorTypes[index].ID;
                            inputTextDrawer.Content = typeId;
                        };
                    }
                    inputTextDrawer.EndEdit += i =>
                    {
                        OnDrawerRefresh();
                    };
                    OnDrawerRefresh();
                    break;
                }
            }
        }
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

                    void OnDrawerRefresh()
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
                        OnDrawerRefresh();
                    };
                    OnDrawerRefresh();
                    break;
                }
                case "SeidDrawer":
                {
                    var seidDrawer = AddEditorDrawer<UIComTextWithContentDrawer>();
                    seidDrawer.Title = "特性描述";
                    
                    void OnDrawerRefresh()
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
                        OnDrawerRefresh();
                    };
                    OnDrawerRefresh();
                    break;
                }
                case "BuffTypeDrawer":
                {
                    var seidDrawer = AddEditorDrawer<UIComDropdownDrawer>();
                    seidDrawer.Title = "Buff类型";
                    
                    void OnDrawerRefresh()
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
                        OnDrawerRefresh();
                    };
                    OnDrawerRefresh();
                    break;
                }
                case "BuffRemoveTriggerTypeDrawer":
                {
                    var seidDrawer = AddEditorDrawer<UIComDropdownDrawer>();
                    seidDrawer.Title = "Buff移除类型";
                    
                    void OnDrawerRefresh()
                    {
                        var typeValue = sInt.Value;
                        var typeList = ModMgr.Instance.BuffDataRemoveTriggerTypes
                            .Select(type => $"{type.ID} {type.Desc}").ToList();
                        seidDrawer.SetOptions(typeList);
                        var typeIndex = ModMgr.Instance.BuffDataRemoveTriggerTypes.FindIndex(type => type.ID == typeValue);
                        seidDrawer.Select(typeIndex);
                        seidDrawer.ValueChange = index =>
                        {
                            var typeId = ModMgr.Instance.BuffDataRemoveTriggerTypes[index].ID;
                            inputNumberDrawer.Content = typeId.ToString();
                        };
                    }

                    inputNumberDrawer.EndEdit += i =>
                    {
                        OnDrawerRefresh();
                    };
                    OnDrawerRefresh();
                    break;
                }
                case "AttackTypeDrawer":
                {
                    var seidDrawer = AddEditorDrawer<UIComDropdownDrawer>();
                    seidDrawer.Title = "攻击类型";
                    
                    void OnDrawerRefresh()
                    {
                        var typeValue = sInt.Value;
                        var typeList = ModMgr.Instance.AttackTypes
                            .Select(type => $"{type.ID} {type.Desc}").ToList();
                        seidDrawer.SetOptions(typeList);
                        var typeIndex = ModMgr.Instance.AttackTypes.FindIndex(type => type.ID == typeValue);
                        seidDrawer.Select(typeIndex);
                        seidDrawer.ValueChange = index =>
                        {
                            var typeId = ModMgr.Instance.AttackTypes[index].ID;
                            inputNumberDrawer.Content = typeId.ToString();
                        };
                    }

                    inputNumberDrawer.EndEdit += i =>
                    {
                        OnDrawerRefresh();
                    };
                    OnDrawerRefresh();
                    break;
                }
                case "ElementTypeDrawer":
                {
                    var attackTypeDrawer = AddEditorDrawer<UIComDropdownDrawer>();
                    attackTypeDrawer.Title = "攻击类型";
                    
                    void OnDrawerRefresh()
                    {
                        var typeValue = sInt.Value;
                        var typeList = ModMgr.Instance.ElementTypes
                            .Select(type => $"{type.ID} {type.Desc}").ToList();
                        attackTypeDrawer.SetOptions(typeList);
                        var typeIndex = ModMgr.Instance.ElementTypes.FindIndex(type => type.ID == typeValue);
                        attackTypeDrawer.Select(typeIndex);
                        attackTypeDrawer.ValueChange = index =>
                        {
                            var typeId = ModMgr.Instance.ElementTypes[index].ID;
                            inputNumberDrawer.Content = typeId.ToString();
                        };
                    }

                    inputNumberDrawer.EndEdit += i =>
                    {
                        OnDrawerRefresh();
                    };
                    OnDrawerRefresh();
                    break;
                }
                case "TargetTypeDrawer":
                {
                    var targetDrawer = AddEditorDrawer<UIComDropdownDrawer>();
                    targetDrawer.Title = "目标类型";
                    
                    void OnDrawerRefresh()
                    {
                        var typeValue = sInt.Value;
                        var typeList = ModMgr.Instance.TargetTypes
                            .Select(type => $"{type.ID} {type.Desc}").ToList();
                        targetDrawer.SetOptions(typeList);
                        var typeIndex = ModMgr.Instance.TargetTypes.FindIndex(type => type.ID == typeValue);
                        targetDrawer.Select(typeIndex);
                        targetDrawer.ValueChange = index =>
                        {
                            var typeId = ModMgr.Instance.TargetTypes[index].ID;
                            inputNumberDrawer.Content = typeId.ToString();
                        };
                    }

                    inputNumberDrawer.EndEdit += i =>
                    {
                        OnDrawerRefresh();
                    };
                    OnDrawerRefresh();
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
                    seidDrawer.Title = "特性描述";

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
        UIMgr.Instance.DestroyPanel<UIModSeidEditorBoxPanel>();
        OnCloseCallback?.Invoke();
    }

    public static void ShowEditor(ModSeidData seidData, ModSeidMeta seidMeta,string title, 
        string okText = "确定", Action callback = null)
    {
        var editor = UIMgr.Instance.GetPanel<UIModSeidEditorBoxPanel>(UILayer.Model);
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