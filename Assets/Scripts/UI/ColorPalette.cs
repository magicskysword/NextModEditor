using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "UI Res/Color Palette")]
public class ColorPalette : ScriptableObject
{
    /// <summary>
    /// 暗系主色
    /// </summary>
    public Color darkPrimaryColor = Color.white;

    /// <summary>
    /// 亮系主色
    /// </summary>
    public Color lightPrimaryColor = Color.white;

    /// <summary>
    /// 主色
    /// </summary>
    public Color primaryColor = Color.white;
    
    /// <summary>
    /// 背景色
    /// </summary>
    public Color backgroundColor = Color.white;

    /// <summary>
    /// 图标颜色
    /// </summary>
    public Color iconColor = Color.white;

    /// <summary>
    /// 强调色
    /// </summary>
    public Color accentColor = Color.white;

    /// <summary>
    /// 文字主色
    /// </summary>
    public Color primaryTextColor = Color.white;

    /// <summary>
    /// 文字辅色
    /// </summary>
    public Color secondaryTextColor = Color.white;

    /// <summary>
    /// 分割色
    /// </summary>
    public Color dividerColor = Color.white;
    /// <summary>
    /// 按钮主色
    /// </summary>
    public Color buttonColor = Color.white;
    /// <summary>
    /// 确认按钮色
    /// </summary>
    public Color okButtonColor = Color.white;
    /// <summary>
    /// 退出按钮色
    /// </summary>
    public Color quitButtonColor = Color.white;
}