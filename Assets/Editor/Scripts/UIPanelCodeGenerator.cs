using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TMPro;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;
using VirtualList;


public static class UIPanelCodeGenerator
{
    [MenuItem("GameObject/UIGen/生成Panel绑定", true)]
    public static bool ValidateGenerateUIPanelBinding()
    {
        var selectionPanel = Selection.activeGameObject;
        return selectionPanel != null && !EditorUtility.IsPersistent(selectionPanel);
    }

    [MenuItem("GameObject/UIGen/生成Panel绑定", false, priority = 20)]
    public static void GenerateUIPanelBinding()
    {
        var selectionPanel = Selection.activeGameObject;

        var templateBindingField =
            AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Editor/Template/UIBindingField.txt").text;
        var templateInitField =
            AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Editor/Template/UIInitField.txt").text;
        var templatePanel =
            AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Editor/Template/UIPanelTemplate.txt").text;

        var panelClassName = $"UI{selectionPanel.name.FirstLetterToUpper()}Panel";
        var comDic = GetBindingDictionary(selectionPanel);

        var fieldStrBuilder = new StringBuilder();
        var fieldInitStrBuilder = new StringBuilder();

        foreach (var pair in comDic)
        {
            var fieldName = pair.Key.Substring(2).FirstLetterToLower();
            var fieldType = pair.Value.Name;
            var comName = pair.Key;

            var field = templateBindingField
                .Replace("%FieldType%", fieldType)
                .Replace("%FieldName%", fieldName);
            fieldStrBuilder.Append(field);
            fieldStrBuilder.Append('\n');

            var fieldInit = templateInitField
                .Replace("%FieldType%", fieldType)
                .Replace("%FieldName%", fieldName)
                .Replace("%ComName%", comName);
            fieldInitStrBuilder.Append(fieldInit);
            fieldInitStrBuilder.Append('\n');
        }

        var panelGenCode = templatePanel
            .Replace("%BindingField%", fieldStrBuilder.ToString())
            .Replace("%OnInit%", fieldInitStrBuilder.ToString())
            .Replace("%UIPanelName%", selectionPanel.name)
            .Replace("%UIPanel%", panelClassName);

        Directory.CreateDirectory("Assets/Scripts/UIPanelGen");
        File.WriteAllText($"Assets/Scripts/UIPanelGen/{panelClassName}.cs", panelGenCode);

        if (PrefabUtility.IsOutermostPrefabInstanceRoot(selectionPanel))
        {
            PrefabUtility.ApplyPrefabInstance(selectionPanel, InteractionMode.UserAction);
        }
        else
        {
            var localPath = "Assets/Resources/UI/" + selectionPanel.name + ".prefab";
            localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);
            PrefabUtility.SaveAsPrefabAssetAndConnect(selectionPanel, localPath, InteractionMode.UserAction);
        }

        EditorUtility.SetDirty(selectionPanel);
        AssetDatabase.Refresh();

        Debug.Log($"生成 UIPanel {panelClassName} 绑定代码成功");
    }

    [MenuItem("GameObject/UIGen/生成Com绑定", true)]
    public static bool ValidateGenerateUIComBinding()
    {
        var selectionCom = Selection.activeGameObject;
        return selectionCom != null && !EditorUtility.IsPersistent(selectionCom);
    }

    [MenuItem("GameObject/UIGen/生成Com绑定", false, priority = 21)]
    public static void GenerateUIComBinding()
    {
        var selectionCom = Selection.activeGameObject;

        var templateBindingField =
            AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Editor/Template/UIBindingField.txt").text;
        var templateInitField =
            AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Editor/Template/UIInitField.txt").text;
        var templateCom =
            AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Editor/Template/UIComTemplate.txt").text;

        var uiComClassName = $"UICom{selectionCom.name.FirstLetterToUpper()}";
        var comDic = GetBindingDictionary(selectionCom);

        var fieldStrBuilder = new StringBuilder();
        var fieldInitStrBuilder = new StringBuilder();

        foreach (var pair in comDic)
        {
            var fieldName = pair.Key.Substring(2).FirstLetterToLower();
            var fieldType = pair.Value.Name;
            var comName = pair.Key;

            var field = templateBindingField
                .Replace("%FieldType%", fieldType)
                .Replace("%FieldName%", fieldName);
            fieldStrBuilder.Append(field);
            fieldStrBuilder.Append('\n');

            var fieldInit = templateInitField
                .Replace("%FieldType%", fieldType)
                .Replace("%FieldName%", fieldName)
                .Replace("%ComName%", comName);
            fieldInitStrBuilder.Append(fieldInit);
            fieldInitStrBuilder.Append('\n');
        }

        var uiComGenCode = templateCom
            .Replace("%BindingField%", fieldStrBuilder.ToString())
            .Replace("%OnInit%", fieldInitStrBuilder.ToString())
            .Replace("%UIComName%", selectionCom.name)
            .Replace("%UICom%", uiComClassName);

        Directory.CreateDirectory("Assets/Scripts/UIComGen");
        File.WriteAllText($"Assets/Scripts/UIComGen/{uiComClassName}.cs", uiComGenCode);

        if (PrefabUtility.IsOutermostPrefabInstanceRoot(selectionCom))
        {
            PrefabUtility.ApplyPrefabInstance(selectionCom, InteractionMode.UserAction);
        }
        else
        {
            var localPath = "Assets/Resources/UICom/" + selectionCom.name + ".prefab";
            localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);
            PrefabUtility.SaveAsPrefabAssetAndConnect(selectionCom, localPath, InteractionMode.UserAction);
        }

        EditorUtility.SetDirty(selectionCom);
        AssetDatabase.Refresh();

        Debug.Log($"生成 UICom {uiComClassName} 绑定代码成功");
    }

    private static Dictionary<string, Type> GetBindingDictionary(GameObject selectionPanel)
    {
        var comDic = new Dictionary<string, Type>();

        var allComponent = selectionPanel.transform.GetComponentsInChildren<Transform>(true);
        foreach (var com in allComponent)
        {
            var comName = com.name;

            if (!comName.StartsWith("g:", StringComparison.OrdinalIgnoreCase))
                continue;

            if (comDic.ContainsKey(comName))
                throw new ArgumentException($"{selectionPanel.name}中存在重复的组件名：{comName}");

            if (comName.StartsWith("g:bg", StringComparison.OrdinalIgnoreCase)
                || comName.StartsWith("img", StringComparison.OrdinalIgnoreCase))
            {
                comDic.Add(comName, typeof(Image));
            }
            else if (comName.StartsWith("g:txt", StringComparison.OrdinalIgnoreCase))
            {
                comDic.Add(comName, typeof(TextMeshProUGUI));
            }
            else if (comName.StartsWith("g:in", StringComparison.OrdinalIgnoreCase))
            {
                comDic.Add(comName, typeof(TMP_InputField));
            }
            else if (comName.StartsWith("g:gp", StringComparison.OrdinalIgnoreCase))
            {
                comDic.Add(comName, typeof(CanvasGroup));
            }
            else if (comName.StartsWith("g:tglGrp", StringComparison.OrdinalIgnoreCase))
            {
                comDic.Add(comName, typeof(ToggleGroup));
            }
            else if (comName.StartsWith("g:tgl", StringComparison.OrdinalIgnoreCase))
            {
                comDic.Add(comName, typeof(Toggle));
            }
            else if (comName.StartsWith("g:lst", StringComparison.OrdinalIgnoreCase))
            {
                comDic.Add(comName, typeof(ScrollRect));
            }
            else if (comName.StartsWith("g:vlst", StringComparison.OrdinalIgnoreCase))
            {
                comDic.Add(comName, typeof(AbstractVirtualList));
            }
            else if (comName.StartsWith("g:rolst", StringComparison.OrdinalIgnoreCase))
            {
                comDic.Add(comName, typeof(ReorderableList));
            }
            else if (comName.StartsWith("g:sli", StringComparison.OrdinalIgnoreCase))
            {
                comDic.Add(comName, typeof(Slider));
            }
            else if (comName.StartsWith("g:ddl", StringComparison.OrdinalIgnoreCase))
            {
                comDic.Add(comName, typeof(TMP_Dropdown));
            }
            else if (comName.StartsWith("g:btn", StringComparison.OrdinalIgnoreCase))
            {
                comDic.Add(comName, typeof(Button));
            }
            else
            {
                comDic.Add(comName, typeof(RectTransform));
            }
        }

        return comDic;
    }

    /// <summary>
    /// Returns the input string with the first character converted to uppercase, or mutates any nulls passed into string.Empty
    /// </summary>
    public static string FirstLetterToUpper(this string s)
    {
        if (string.IsNullOrEmpty(s))
            return string.Empty;

        char[] a = s.ToCharArray();
        a[0] = char.ToUpper(a[0]);
        return new string(a);
    }

    /// <summary>
    /// Returns the input string with the first character converted to uppercase, or mutates any nulls passed into string.Empty
    /// </summary>
    public static string FirstLetterToLower(this string s)
    {
        if (string.IsNullOrEmpty(s))
            return string.Empty;

        char[] a = s.ToCharArray();
        a[0] = char.ToLower(a[0]);
        return new string(a);
    }
}
