using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class TextFloatUnderline : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    private TextMeshProUGUI txt;
    
    private void OnEnable()
    {
        txt = GetComponent<TextMeshProUGUI>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        txt.fontStyle |= FontStyles.Underline;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        txt.fontStyle &= (~ FontStyles.Underline);
    }
}