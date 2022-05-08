public interface IModEditor
{
    public ModProject BindProject { get; set; }
}

public interface IModDataEditor : IModEditor
{
    public IModData SelectModData { get; }
    public UIComModCommonEditor CommonEditor { get; }
}