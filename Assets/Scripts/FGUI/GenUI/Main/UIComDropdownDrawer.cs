/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace UIPkg.Main
{
    public partial class UIComDropdownDrawer : GLabel
    {
        public GComboBox m_dropdown;
        public const string URL = "ui://jcakb2cpof7d2i";

        public static UIComDropdownDrawer CreateInstance()
        {
            return (UIComDropdownDrawer)UIPackage.CreateObject("Main", "ComDropdownDrawer");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_dropdown = (GComboBox)GetChild("dropdown");
        }
    }
}