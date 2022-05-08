/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace UIPkg.Main
{
    public partial class UIWindowFrame : GLabel
    {
        public Controller m_hasCloseButton;
        public GGraph m_dragArea;
        public GGraph m_contentArea;
        public GButton m_closeButton;
        public const string URL = "ui://jcakb2cptcay23";

        public static UIWindowFrame CreateInstance()
        {
            return (UIWindowFrame)UIPackage.CreateObject("Main", "WindowFrame");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_hasCloseButton = GetController("hasCloseButton");
            m_dragArea = (GGraph)GetChild("dragArea");
            m_contentArea = (GGraph)GetChild("contentArea");
            m_closeButton = (GButton)GetChild("closeButton");
        }
    }
}