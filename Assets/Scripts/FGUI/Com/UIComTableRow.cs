using System.Collections.Generic;

namespace UIPkg.Main
{
    public partial class UIComTableRow
    {
        public void RefreshItem(IEnumerable<TableInfo> infos,object getData,float minWidth)
        {
            m_list.RemoveChildrenToPool();
            foreach (var tableItemInfo in infos)
            {
                var item = m_list.AddItemFromPool().asLabel;
                item.title = tableItemInfo.Getter.Invoke(getData);
                item.width = tableItemInfo.Width;
            }
            m_list.ResizeToFit();
            if (m_list.width < minWidth)
            {
                width = minWidth;
            }
            else
            {
                width = m_list.width;
            }
        }
    }
}