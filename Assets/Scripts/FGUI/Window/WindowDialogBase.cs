using FairyGUI;
using UnityEngine;

public class WindowDialogBase : Window
{
    protected override void DoShowAnimation()
    {
        pivot = new Vector2(0.5f, 0.5f);
        alpha = 0f;
        scale = Vector2.zero;
        Center();
        TweenScale(Vector2.one, 0.3f).SetEase(EaseType.CubicOut);
        TweenFade(1f, 0.3f).OnComplete(OnShown);
    }

    protected override void OnHide()
    {
        Dispose();
    }
}