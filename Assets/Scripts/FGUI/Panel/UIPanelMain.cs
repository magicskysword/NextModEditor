using System;
using System.Collections.Generic;
using FairyGUI;
using UnityEngine;

namespace UIPkg.Main
{
    public partial class UIPanelMain : IFGUIInit
    {
        private ModProject Project { get; set; }
        
        private EventCallback1 _onRemoveTabItem;
        private PopupMenu _filePopMenu = new PopupMenu();
        private List<PanelTab> Tabs { get; } = new List<PanelTab>();
        private List<UIProjectBase> ProjectItems { get; } = new List<UIProjectBase>();
        
        private int _curTabIndex;

        public void OnInit()
        {
            InitPopMenu();
            InitHeader();
            InitProject();
            InitDocumentView();
            InitSeg();
        }

        private void InitPopMenu()
        {
            var btnOpen = _filePopMenu.AddItem("main.header.file.open".I18N(), OnOpenModProject);
            btnOpen.name = "itemOpen";
            var btnSave = _filePopMenu.AddItem("main.header.file.save".I18N(), () => { });
            btnSave.name = "itemSave";
            var btnExport = _filePopMenu.AddItem("main.header.file.export".I18N(), () => { });
            btnExport.name = "itemExport";
        }

        private void InitHeader()
        {
            var btnFile = m_comHeader.m_lstHeader.AddItemFromPool().asButton;
            btnFile.title = "main.header.file".I18N();
            btnFile.onClick.Add(()=>_filePopMenu.Show(btnFile,PopupDirection.Down));
        }
        
        private void InitProject()
        {
            m_comProject.m_treeView.treeNodeRender = ProjectTreeItemRenderer;
            m_comProject.m_treeView.onClickItem.Set(OnClickProjectTreeItem);
            
            AddProject("main.project.modConfig".I18N(),0,new UIProjectItemModConfig());
            AddProject("main.project.modCreateAvatar".I18N(),0,new UIProjectItemModCreateAvatar());
            AddProject("main.project.modBuffInfo".I18N(),0,new UIProjectItemModBuffInfo());
            
        }

        private void InitDocumentView()
        {
            _onRemoveTabItem = OnRemoveTabItem;
            
            m_comDocument.m_lstTab.itemRenderer = TabItemRenderer;
            m_comDocument.m_lstTab.onClickItem.Set(OnClickTabItem);
        }
        
        private void InitSeg()
        {
            m_leftSeg.draggable = true;
            m_leftSeg.onDragStart.Set(OnDragLeftSegStart);
            m_leftSeg.cursor = "resizeH";

            m_rightSeg.draggable = true;
            m_rightSeg.onDragStart.Set(OnDragRightSegStart);
            m_rightSeg.cursor = "resizeH";
        }

        private void OnDragLeftSegStart(EventContext context)
        {
            context.PreventDefault();
            DragDropManager.inst.StartDrag(null, null,null, (int)context.data);
            DragDropManager.inst.dragAgent.onDragMove.Set(OnDragLeftSegMove);
        }
        
        private void OnDragRightSegStart(EventContext context)
        {
            context.PreventDefault();
            DragDropManager.inst.StartDrag(null, null,null, (int)context.data);
            DragDropManager.inst.dragAgent.onDragMove.Set(OnDragRightSegMove);
        }
        
        private void OnDragLeftSegMove(EventContext context)
        {
            var posX = context.inputEvent.x;
            posX = Mathf.Clamp(posX, 50, m_rightSeg.x - 50);
            m_leftSeg.x = posX;
        }
        
        private void OnDragRightSegMove(EventContext context)
        {
            var posX = context.inputEvent.x;
            posX = Mathf.Clamp(posX, m_leftSeg.x + 50, x + width - 50);
            m_rightSeg.x = posX;
        }

        #region HeadbarFunction

        private void OnOpenModProject()
        {
            try
            {
                var project = ModMgr.I.OpenProject();
                if (project != null)
                {
                    Project = project;
                    EventCenter.Send(new LoadModProjectEventArgs()
                    {
                        ModProject = project
                    });
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        #endregion

        #region DocumentFunction

        private void OnClickTabItem(EventContext context)
        {
            var item = (UIBtnTab)context.data;
            var index = m_comDocument.m_lstTab.GetChildIndex(item);
            OnSwitchTab(index);
        }

        private void OnSwitchTab(int index, bool forceRefresh = false)
        {
            if (_curTabIndex == index && !forceRefresh)
            {
                return;
            }
            Debug.Log($"切换Tab");
            _curTabIndex = index;
            m_comDocument.m_lstTab.selectedIndex = _curTabIndex;

            m_comDocument.m_content.RemoveChildren();
            m_comInspector.Clear();

            if (index >= 0)
            {
                var tab = Tabs[index];
                m_comDocument.m_content.AddChild(tab.Content);
                tab.OnOpen();
            }
        }

        private void OnRemoveTabItem(EventContext context)
        {
            var btn = (GButton)context.sender;
            var tabItem = (UIBtnTab)btn.parent;
            var tab = (PanelTab)tabItem.data;
            RemoveTab(tab);
        }

        private void TabItemRenderer(int index, GObject item)
        {
            var tabItem = (UIBtnTab)item;
            var tabData = Tabs[index];

            tabItem.title = tabData.Name;
            tabItem.data = tabData;
            tabItem.m_closeButton.onClick.Set(_onRemoveTabItem);
        }

        private void RefreshTabs()
        {
            m_comDocument.m_lstTab.numItems = Tabs.Count;
        }

        private bool TryGetTab(string tabID,out PanelTab tab)
        {
            tab = Tabs.Find(tab => tab.ID == tabID);
            return tab != null;
        }
        
        private bool HasTab(string tabID)
        {
            return Tabs.Find(tab => tab.ID == tabID) != null;
        }
        
        private void AddTab(PanelTab tab)
        {
            Tabs.Add(tab);
            
            tab.Inspector = m_comInspector;
            tab.Project = Project;
            tab.OnAdd();
            
            RefreshTabs();
        }

        private void RemoveTab(PanelTab tab)
        {
            var lstTab = m_comDocument.m_lstTab;
            var oldIndex = lstTab.selectedIndex;
            var newIndex = 0;
            Tabs.Remove(tab);
            tab.OnRemove();
            RefreshTabs();
            if (Tabs.Count > 0)
            {
                if (oldIndex > 0)
                    newIndex = oldIndex - 1;
                else
                    newIndex = 0;
            }
            else
            {
                newIndex = -1;
            }

            OnSwitchTab(newIndex);
        }

        private void SelectTab(PanelTab tab)
        {
            var index = Tabs.IndexOf(tab);
            OnSwitchTab(index,true);
        }

        #endregion

        #region ProjectFunction

        private void AddProject(string projName,int projLayer,UIProjectBase uiProjectBase)
        {
            uiProjectBase.Name = projName;
            uiProjectBase.Layer = projLayer;
            ProjectItems.Add(uiProjectBase);

            var node = new GTreeNode(!uiProjectBase.IsLeaf);
            node.data = uiProjectBase;
            m_comProject.m_treeView.rootNode.AddChild(node);
        }
        
        private void OnClickProjectTreeItem(EventContext context)
        {
            if(!context.inputEvent.isDoubleClick)
                return;
            
            if(Project == null)
                return;
            
            var obj = (GObject)context.data;
            var node = obj.treeNode;
            var uiProjectBase = (UIProjectBase)node.data;

            if (uiProjectBase is UIProjectItem uiProjectItem)
            {
                if (!TryGetTab(uiProjectItem.ID,out var tab))
                {
                    tab = uiProjectItem.CreateTab();
                    tab.ID = uiProjectItem.ID;
                    AddTab(tab);
                }
                SelectTab(tab);
            }
        }
        
        private void ProjectTreeItemRenderer(GTreeNode node, GComponent obj)
        {
            var uiProjectBase = (UIProjectBase)node.data;
            var treeItem = (UIBtnTreeItem)obj;

            treeItem.title = uiProjectBase.Name;
            if (uiProjectBase is UIProjectItem uiProjectItem)
            {
                if (!string.IsNullOrEmpty(uiProjectItem.Icon))
                {
                    treeItem.icon = uiProjectItem.Icon;
                }
                else
                {
                    treeItem.icon = "ui://Main/icon_tool1";
                }
            }
        }

        #endregion
    }
}