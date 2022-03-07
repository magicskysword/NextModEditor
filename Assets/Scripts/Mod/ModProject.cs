using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

public class ModProject
{
    public string ProjectPath { get; set; }
    public ModConfig Config { get; set; }
    
    public List<ModCreateAvatarData> CreateAvatarData { get; set; }
    public ModCreateAvatarSeidDataGroup CreateAvatarSeidDataGroup { get; set; }
    public List<ModBuffData> BuffData { get; set; }
    public ModBuffSeidDataGroup BuffSeidDataGroup { get; set; }
    public List<ModAffixData> AffixData { get; set; }
    
    public ModAffixData FindAffix(int affixID)
    {
        var affixData = AffixData.Find(data => data.ID == affixID);
        
        if(affixData == null)
        {
            ModMgr.Instance.DefaultAffixData.TryGetValue(affixID,out affixData);
        }

        return affixData;
    }
    
    public ModBuffData FindBuff(int buffID)
    {
        var buffData = BuffData.Find(data => data.ID == buffID);
        
        if(buffData == null)
        {
            ModMgr.Instance.DefaultBuffData.TryGetValue(buffID,out buffData);
        }

        return buffData;
    }

    public static ModProject Load(string dir)
    {
        ModProject project = new ModProject
        {
            ProjectPath = dir,
            Config = ModConfig.Load(dir),
            CreateAvatarData = ModCreateAvatarData.Load(dir)?.Select(pair=>pair.Value).ToList(),
            CreateAvatarSeidDataGroup = ModCreateAvatarSeidDataGroup.Load(dir,ModMgr.Instance.CreateAvatarSeidMetas),
            BuffData = ModBuffData.Load(dir),
            BuffSeidDataGroup = ModBuffSeidDataGroup.Load(dir,ModMgr.Instance.BuffSeidMetas),
            AffixData = ModAffixData.Load(dir).Select(pair=>pair.Value).ToList(),
        };

        project.CreateAvatarData.ModSort();
        project.BuffData.ModSort();
        project.AffixData.ModSort();

        return project;
    }

    public static void Save(string dir,ModProject project)
    {
        ModConfig.Save(dir, project.Config);
        ModCreateAvatarData.Save(dir, project.CreateAvatarData.ToDictionary(item => item.ID.ToString()));
        ModCreateAvatarSeidDataGroup.Save(dir, project.CreateAvatarSeidDataGroup);
        ModBuffData.Save(dir,project.BuffData);
        ModBuffSeidDataGroup.Save(dir, project.BuffSeidDataGroup);
        ModAffixData.Save(dir, project.AffixData.ToDictionary(item => item.ID.ToString()));
    }
}