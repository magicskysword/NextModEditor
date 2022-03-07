using System;
using UnityEngine;
using UnityEngine.UI;

public class ScrollbarMinSize : MonoBehaviour
{
    private Scrollbar Scrollbar;
    
    public float minSize;

    private void Awake()
    {
        Scrollbar = GetComponent<Scrollbar>();
    }

    public void LateUpdate()
    {
        Scrollbar.size = Mathf.Max(Scrollbar.size, minSize);
    }
}