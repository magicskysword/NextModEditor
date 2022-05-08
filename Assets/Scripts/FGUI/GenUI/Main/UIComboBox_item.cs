/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace UIPkg.Main
{
    public partial class UIComboBox_item : GButton
    {
        public GGraph m_selected;
        public GGraph m_hover;
        public const string URL = "ui://jcakb2cpof7d2f";

        public static UIComboBox_item CreateInstance()
        {
            return (UIComboBox_item)UIPackage.CreateObject("Main", "ComboBox_item");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_selected = (GGraph)GetChild("selected");
            m_hover = (GGraph)GetChild("hover");
        }
    }
}