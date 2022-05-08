using System.Collections.Generic;

namespace UIPkg.Main
{
    public partial class UIComMainInspector
    {
        private List<ModPropertyDrawer> _drawers = new List<ModPropertyDrawer>();

        public void AddDrawer(ModPropertyDrawer drawer)
        {
            _drawers.Add(drawer);
            m_list.AddChild(drawer.CreateCom());
        }

        public void Clear()
        {
            m_list.RemoveChildren();
            foreach (var drawer in _drawers)
            {
                drawer.RemoveCom();
            }
            _drawers.Clear();
        }

        public void Refresh()
        {
            foreach (var drawer in _drawers)
            {
                drawer.Refresh();
            }
        }
    }
}