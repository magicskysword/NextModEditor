/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace UIPkg.Main
{
    public partial class UIComStringAreaDrawer : GLabel
    {
        public GTextInput m_inContent;
        public GButton m_btnEdit;
        public const string URL = "ui://jcakb2cp10zy01x";

        public static UIComStringAreaDrawer CreateInstance()
        {
            return (UIComStringAreaDrawer)UIPackage.CreateObject("Main", "ComStringAreaDrawer");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_inContent = (GTextInput)GetChild("inContent");
            m_btnEdit = (GButton)GetChild("btnEdit");
        }
    }
}