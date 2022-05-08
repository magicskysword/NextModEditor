using UniRx;

public partial class UIModConfigEditorPanel : IModEditor
{
    private ModProject _bindProject;

    public ModProject BindProject
    {
        get => _bindProject;
        set
        {
            _bindProject = value;
            RefreshUI();
        }
    }

    protected override void OnInit()
    {
        inModName.OnEndEditAsObservable()
            .Subscribe(str =>
            {
                if (BindProject != null)
                {
                    BindProject.Config.Name = str;
                }
            });
        
        inModAuthor.OnEndEditAsObservable()
            .Subscribe(str =>
            {
                if (BindProject != null)
                {
                    BindProject.Config.Author = str;
                }
            });
        
        inModVersion.OnEndEditAsObservable()
            .Subscribe(str =>
            {
                if (BindProject != null)
                {
                    BindProject.Config.Version = str;
                }
            });
        
        inModDesc.OnEndEditAsObservable()
            .Subscribe(str =>
            {
                if (BindProject != null)
                {
                    BindProject.Config.Description = str;
                }
            });
    }

    protected void RefreshUI()
    {
        inModName.text = BindProject?.Config.Name;
        inModAuthor.text = BindProject?.Config.Author;
        inModVersion.text = BindProject?.Config.Version;
        inModDesc.text = BindProject?.Config.Description;
    }
}