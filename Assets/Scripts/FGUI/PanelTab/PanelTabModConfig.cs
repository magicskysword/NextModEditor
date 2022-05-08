using FairyGUI;
using UnityEngine;

public class PanelTabModConfig : PanelTabEmpty
{
    public override void OnOpen()
    {
        var config = Project.Config;
        
        Inspector.AddDrawer(new ModStringPropertyDrawer(
            "main.modConfig.name".I18N(),
            str => config.Name = str, 
            () => config.Name)
        );  
        
        Inspector.AddDrawer(new ModStringPropertyDrawer(
            "main.modConfig.author".I18N(),
            str => config.Author = str, 
            () => config.Author)
        );
        
        Inspector.AddDrawer(new ModStringPropertyDrawer(
            "main.modConfig.version".I18N(),
            str => config.Version = str, 
            () => config.Version)
        );
        
        Inspector.AddDrawer(new ModStringAreaPropertyDrawer(
            "main.modConfig.desc".I18N(),
            str => config.Description = str, 
            () => config.Description)
        );
        
        Inspector.Refresh();
    }
}