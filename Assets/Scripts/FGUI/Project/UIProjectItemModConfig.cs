public class UIProjectItemModConfig : UIProjectItem
{
    public override PanelTab CreateTab()
    {
        return new PanelTabModConfig()
        {
            Name = Name
        };
    }
}