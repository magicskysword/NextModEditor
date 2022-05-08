/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace UIPkg.Main
{
    public partial class UIComIntDrawer : GLabel
    {
        public Controller m_warning;
        public GTextInput m_inContent;
        public const string URL = "ui://jcakb2cp10zy01v";

        public static UIComIntDrawer CreateInstance()
        {
            return (UIComIntDrawer)UIPackage.CreateObject("Main", "ComIntDrawer");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_warning = GetController("warning");
            m_inContent = (GTextInput)GetChild("inContent");
        }
    }
}