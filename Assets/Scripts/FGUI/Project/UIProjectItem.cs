using System;

public abstract class UIProjectItem : UIProjectBase
{
    public virtual string ID => this.GetType().FullName;
    public abstract PanelTab CreateTab();
    public override bool IsLeaf => true;
    public virtual string Icon { get; } = String.Empty;
}