/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace UIPkg.Main
{
    public partial class UIWinConfirmDialog : GComponent
    {
        public UIWindowFrame m_frame;
        public GTextField m_text;
        public GButton m_btnOk;
        public GButton m_closeButton;
        public const string URL = "ui://jcakb2cptcay2b";

        public static UIWinConfirmDialog CreateInstance()
        {
            return (UIWinConfirmDialog)UIPackage.CreateObject("Main", "WinConfirmDialog");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_frame = (UIWindowFrame)GetChild("frame");
            m_text = (GTextField)GetChild("text");
            m_btnOk = (GButton)GetChild("btnOk");
            m_closeButton = (GButton)GetChild("closeButton");
        }
    }
}