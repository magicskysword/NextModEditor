using FairyGUI;
using UIPkg.Main;

public abstract class PanelTab
{
    public string ID { get; set; }
    public string Name { get; set; }
    public ModProject Project { get; set; }
    public UIComMainInspector Inspector { get; set; }
    public GObject Content { get; set; }
    
    public abstract void OnAdd();
    public abstract void OnOpen();
    public abstract void OnRemove();
}