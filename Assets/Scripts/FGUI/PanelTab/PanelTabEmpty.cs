using FairyGUI;

public abstract class PanelTabEmpty : PanelTab
{
    public override void OnAdd()
    {
        var graph = new GGraph();
        Content = graph;
    }

    public override void OnRemove()
    {
        Content.Dispose();
    }
}