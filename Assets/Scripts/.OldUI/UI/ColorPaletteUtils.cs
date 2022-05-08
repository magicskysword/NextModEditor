using UnityEngine;

public static class ColorPaletteUtils
{
    public static ColorPalette GetPalette(string paletteName)
    {
        return Resources.Load<ColorPalette>($"Palette/{paletteName}");
    }

    public static ColorPalette GetDefaultPalette()
    {
        return GetPalette("Default");
    }
}