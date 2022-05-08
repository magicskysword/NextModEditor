/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace UIPkg.Main
{
    public partial class UIComMainHeader : GComponent
    {
        public GList m_lstHeader;
        public const string URL = "ui://jcakb2cphbj216";

        public static UIComMainHeader CreateInstance()
        {
            return (UIComMainHeader)UIPackage.CreateObject("Main", "ComMainHeader");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_lstHeader = (GList)GetChild("lstHeader");
        }
    }
}