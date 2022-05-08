/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace UIPkg.Main
{
    public partial class UIComMainProject : GComponent
    {
        public GTextInput m_inSearch;
        public GTree m_treeView;
        public const string URL = "ui://jcakb2cphbj21c";

        public static UIComMainProject CreateInstance()
        {
            return (UIComMainProject)UIPackage.CreateObject("Main", "ComMainProject");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_inSearch = (GTextInput)GetChild("inSearch");
            m_treeView = (GTree)GetChild("treeView");
        }
    }
}