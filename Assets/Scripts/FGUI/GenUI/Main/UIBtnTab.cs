/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace UIPkg.Main
{
    public partial class UIBtnTab : GButton
    {
        public GButton m_closeButton;
        public const string URL = "ui://jcakb2cp10zy01f";

        public static UIBtnTab CreateInstance()
        {
            return (UIBtnTab)UIPackage.CreateObject("Main", "BtnTab");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_closeButton = (GButton)GetChild("closeButton");
        }
    }
}