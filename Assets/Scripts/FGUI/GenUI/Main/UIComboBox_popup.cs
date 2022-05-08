/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace UIPkg.Main
{
    public partial class UIComboBox_popup : GComponent
    {
        public GList m_list;
        public const string URL = "ui://jcakb2cpof7d2g";

        public static UIComboBox_popup CreateInstance()
        {
            return (UIComboBox_popup)UIPackage.CreateObject("Main", "ComboBox_popup");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_list = (GList)GetChild("list");
        }
    }
}