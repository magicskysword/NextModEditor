/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace UIPkg.Main
{
    public partial class UIComIntBindDataDrawer : GLabel
    {
        public Controller m_warning;
        public GTextInput m_inContent;
        public GTextField m_txtDesc;
        public GButton m_btnEdit;
        public const string URL = "ui://jcakb2cpdxyi2c";

        public static UIComIntBindDataDrawer CreateInstance()
        {
            return (UIComIntBindDataDrawer)UIPackage.CreateObject("Main", "ComIntBindDataDrawer");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_warning = GetController("warning");
            m_inContent = (GTextInput)GetChild("inContent");
            m_txtDesc = (GTextField)GetChild("txtDesc");
            m_btnEdit = (GButton)GetChild("btnEdit");
        }
    }
}