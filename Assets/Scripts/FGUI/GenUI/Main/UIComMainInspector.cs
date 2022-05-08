/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace UIPkg.Main
{
    public partial class UIComMainInspector : GComponent
    {
        public GList m_list;
        public const string URL = "ui://jcakb2cp10zy01k";

        public static UIComMainInspector CreateInstance()
        {
            return (UIComMainInspector)UIPackage.CreateObject("Main", "ComMainInspector");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_list = (GList)GetChild("list");
        }
    }
}