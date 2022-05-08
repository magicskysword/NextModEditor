/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace UIPkg.Main
{
    public partial class UILabelTableGridHeader : GLabel
    {
        public GGraph m_dragable;
        public const string URL = "ui://jcakb2cp10zy01s";

        public static UILabelTableGridHeader CreateInstance()
        {
            return (UILabelTableGridHeader)UIPackage.CreateObject("Main", "LabelTableGridHeader");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_dragable = (GGraph)GetChild("dragable");
        }
    }
}