/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace UIPkg.Main
{
    public partial class UIPopupMenu_item : GButton
    {
        public Controller m_checked;
        public GImage m_select;
        public GImage m_arrow;
        public const string URL = "ui://jcakb2cphbj218";

        public static UIPopupMenu_item CreateInstance()
        {
            return (UIPopupMenu_item)UIPackage.CreateObject("Main", "PopupMenu_item");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_checked = GetController("checked");
            m_select = (GImage)GetChild("select");
            m_arrow = (GImage)GetChild("arrow");
        }
    }
}