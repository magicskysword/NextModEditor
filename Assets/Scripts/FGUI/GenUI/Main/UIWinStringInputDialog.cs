/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace UIPkg.Main
{
    public partial class UIWinStringInputDialog : GComponent
    {
        public UIWindowFrame m_frame;
        public GButton m_btnOk;
        public GButton m_closeButton;
        public GTextInput m_inContent;
        public const string URL = "ui://jcakb2cptcay28";

        public static UIWinStringInputDialog CreateInstance()
        {
            return (UIWinStringInputDialog)UIPackage.CreateObject("Main", "WinStringInputDialog");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_frame = (UIWindowFrame)GetChild("frame");
            m_btnOk = (GButton)GetChild("btnOk");
            m_closeButton = (GButton)GetChild("closeButton");
            m_inContent = (GTextInput)GetChild("inContent");
        }
    }
}