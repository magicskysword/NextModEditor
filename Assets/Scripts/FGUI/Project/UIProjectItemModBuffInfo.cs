public class UIProjectItemModBuffInfo : UIProjectItem
{
    public override PanelTab CreateTab()
    {
        return new PanelTabModBuffInfo()
        {
            Name = Name
        };
    }
}