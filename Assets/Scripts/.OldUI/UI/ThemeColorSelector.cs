using System;
using UnityEngine;
using UnityEngine.UI;

public class ThemeColorSelector : MonoBehaviour
{
    /// <summary>
    /// 颜色类型
    /// </summary>
    public ThemeColorType colorType;
    public bool reversalColor;

    public Graphic Graphic => GetComponent<Graphic>();

    public void ChangeColor(ColorPalette palette)
    {
        var graphic = Graphic;
        if (graphic == null)
        {
            return;
        }
        
        switch (colorType)
        {
            case ThemeColorType.DarkPrimary:
                graphic.color = palette.darkPrimaryColor;
                break;
            case ThemeColorType.LightPrimary:
                graphic.color = palette.lightPrimaryColor;
                break;
            case ThemeColorType.Primary:
                graphic.color = palette.primaryColor;
                break;
            case ThemeColorType.Background:
                graphic.color = palette.backgroundColor;
                break;
            case ThemeColorType.Icon:
                graphic.color = palette.iconColor;
                break;
            case ThemeColorType.Accent:
                graphic.color = palette.accentColor;
                break;
            case ThemeColorType.PrimaryText:
                graphic.color = palette.primaryTextColor;
                break;
            case ThemeColorType.SecondaryText:
                graphic.color = palette.secondaryTextColor;
                break;
            case ThemeColorType.Divider:
                graphic.color = palette.dividerColor;
                break;
            case ThemeColorType.Button:
                graphic.color = palette.buttonColor;
                break;
            case ThemeColorType.OkButton:
                graphic.color = palette.okButtonColor;
                break;
            case ThemeColorType.QuitButton:
                graphic.color = palette.quitButtonColor;
                break;
        }

        if (reversalColor)
            graphic.color = new Color(1 - graphic.color.r, 1 - graphic.color.g, 1 - graphic.color.b, graphic.color.a);
    }

    private void OnEnable()
    {
        ChangeColor(ColorPaletteUtils.GetDefaultPalette());
    }

    private void OnColorTypeChange()
    {
        ChangeColor(ColorPaletteUtils.GetDefaultPalette());
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        OnColorTypeChange();
    }
#endif
}