/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace UIPkg.Main
{
    public partial class UIComMainDocumentView : GComponent
    {
        public GList m_lstTab;
        public GList m_content;
        public const string URL = "ui://jcakb2cp10zy01j";

        public static UIComMainDocumentView CreateInstance()
        {
            return (UIComMainDocumentView)UIPackage.CreateObject("Main", "ComMainDocumentView");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_lstTab = (GList)GetChild("lstTab");
            m_content = (GList)GetChild("content");
        }
    }
}