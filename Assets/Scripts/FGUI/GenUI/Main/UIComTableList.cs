/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace UIPkg.Main
{
    public partial class UIComTableList : GComponent
    {
        public GList m_listHeader;
        public GList m_list;
        public const string URL = "ui://jcakb2cp10zy01t";

        public static UIComTableList CreateInstance()
        {
            return (UIComTableList)UIPackage.CreateObject("Main", "ComTableList");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_listHeader = (GList)GetChild("listHeader");
            m_list = (GList)GetChild("list");
        }
    }
}