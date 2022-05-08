/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace UIPkg.Main
{
    public partial class UIPopupMenu : GComponent
    {
        public GList m_list;
        public const string URL = "ui://jcakb2cphbj219";

        public static UIPopupMenu CreateInstance()
        {
            return (UIPopupMenu)UIPackage.CreateObject("Main", "PopupMenu");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_list = (GList)GetChild("list");
        }
    }
}