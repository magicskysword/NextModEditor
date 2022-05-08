﻿using System;

namespace UIPkg.Main
{
    public partial class UIComIntDrawer
    {
        public void BindEndEdit(Action<int> endEdit)
        {
            var ctrl = GetController("warning");
            
            m_inContent.onFocusOut.Set(() =>
            {
                var str = m_inContent.text;
                if(int.TryParse(str,out var num))
                {
                    endEdit?.Invoke(num);
                    ctrl.selectedIndex = 0;
                }
                else
                {
                    ctrl.selectedIndex = 1;
                }
            });
        }
    }
}