using UnityEditor;
using UnityEngine;

public static class ColorPaletteEditorUtils
{
    [MenuItem("Tools/UI/刷新UI主题颜色")]
    public static void RefreshAllThemeColor()
    {
        var allSelectors = GameObject.FindObjectsOfType<ThemeColorSelector>(true);
        foreach (var selector in allSelectors)
        {
            selector.ChangeColor(ColorPaletteUtils.GetDefaultPalette());
            EditorUtility.SetDirty(selector);
        }
        Debug.Log($"刷新UI主题颜色成功");
    }
}