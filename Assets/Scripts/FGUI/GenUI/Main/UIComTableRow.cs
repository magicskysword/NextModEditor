/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace UIPkg.Main
{
    public partial class UIComTableRow : GButton
    {
        public GGraph m_selected;
        public GGraph m_hover;
        public GList m_list;
        public const string URL = "ui://jcakb2cp10zy01p";

        public static UIComTableRow CreateInstance()
        {
            return (UIComTableRow)UIPackage.CreateObject("Main", "ComTableRow");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_selected = (GGraph)GetChild("selected");
            m_hover = (GGraph)GetChild("hover");
            m_list = (GList)GetChild("list");
        }
    }
}