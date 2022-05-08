/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace UIPkg.Main
{
    public partial class UIPanelMain : GComponent
    {
        public UIComMainHeader m_comHeader;
        public UIComMainProject m_comProject;
        public UIComMainDocumentView m_comDocument;
        public UIComMainInspector m_comInspector;
        public GComponent m_comFooter;
        public GGraph m_leftSeg;
        public GGraph m_rightSeg;
        public const string URL = "ui://jcakb2cphbj21a";

        public static UIPanelMain CreateInstance()
        {
            return (UIPanelMain)UIPackage.CreateObject("Main", "PanelMain");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_comHeader = (UIComMainHeader)GetChild("comHeader");
            m_comProject = (UIComMainProject)GetChild("comProject");
            m_comDocument = (UIComMainDocumentView)GetChild("comDocument");
            m_comInspector = (UIComMainInspector)GetChild("comInspector");
            m_comFooter = (GComponent)GetChild("comFooter");
            m_leftSeg = (GGraph)GetChild("leftSeg");
            m_rightSeg = (GGraph)GetChild("rightSeg");
        }
    }
}