/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace UIPkg.Main
{
    public partial class UIComFloatDrawer : GLabel
    {
        public GTextInput m_inContent;
        public const string URL = "ui://jcakb2cp10zy01w";

        public static UIComFloatDrawer CreateInstance()
        {
            return (UIComFloatDrawer)UIPackage.CreateObject("Main", "ComFloatDrawer");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_inContent = (GTextInput)GetChild("inContent");
        }
    }
}