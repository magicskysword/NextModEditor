public class UIProjectItemModCreateAvatar : UIProjectItem
{
    public override PanelTab CreateTab()
    {
        return new PanelTabModCreateAvatar()
        {
            Name = Name
        };
    }
}