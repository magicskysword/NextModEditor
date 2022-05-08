/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace UIPkg.Main
{
    public partial class UIWinSelectorDialog : GComponent
    {
        public UIWindowFrame m_frame;
        public GButton m_btnOk;
        public GButton m_closeButton;
        public GImage m_bg;
        public GTextField m_txtTips;
        public UIComTableList m_table;
        public const string URL = "ui://jcakb2cpdxyi2d";

        public static UIWinSelectorDialog CreateInstance()
        {
            return (UIWinSelectorDialog)UIPackage.CreateObject("Main", "WinSelectorDialog");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_frame = (UIWindowFrame)GetChild("frame");
            m_btnOk = (GButton)GetChild("btnOk");
            m_closeButton = (GButton)GetChild("closeButton");
            m_bg = (GImage)GetChild("bg");
            m_txtTips = (GTextField)GetChild("txtTips");
            m_table = (UIComTableList)GetChild("table");
        }
    }
}