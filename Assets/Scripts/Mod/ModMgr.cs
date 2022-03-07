using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json.Linq;
using SFB;
using UnityEngine;

public class ModMgr
{
    public const string EDITOR_VERSION = "0.1.0";
    public static ModMgr Instance { get; }
    
    public ModProject CurProject { get; set; }
    public List<ModAffixDataProjectType> AffixDataProjectTypes { get; set; }
    public List<ModAffixDataAffixType> AffixDataAffixTypes { get; set; }
    public Dictionary<int,ModSeidMeta> CreateAvatarSeidMetas { get; set; }
    public List<ModCreateAvatarDataTalentType> CreateAvatarDataTalentTypes { get; set; }
    public List<ModCreateAvatarDataLevelType> CreateAvatarDataLevelTypes { get; set; }
    public Dictionary<int,ModSeidMeta> BuffSeidMetas { get; set; }
    public List<ModBuffDataBuffType> BuffDataBuffTypes { get; set; }
    public List<ModBuffDataTriggerType> BuffDataTriggerTypes { get; set; }
    public List<ModBuffDataRemoveTriggerType> BuffDataRemoveTriggerTypes { get; set; }
    public List<ModBuffDataOverlayType> BuffDataOverlayTypes { get; set; }
    public Dictionary<int,ModAffixData> DefaultAffixData { get; set; }
    public Dictionary<int,ModBuffData> DefaultBuffData { get; set; }

    static ModMgr()
    {
        foreach (var type in ModUtils.GetTypesWithAttribute(Assembly.GetAssembly(typeof(ModDataInitAttribute)),
                     typeof(ModDataInitAttribute)))
        {
            var initMethod = type.GetMethod("Init", BindingFlags.Static | BindingFlags.Public);
            initMethod!.Invoke(null, Array.Empty<object>());
        }
        Instance = new ModMgr();
    }

    public void Init()
    {
        AffixDataProjectTypes = JArray
            .Parse(ModUtils.LoadConfig("Meta/AffixProjectType.json"))
            .ToObject<List<ModAffixDataProjectType>>();
        AffixDataAffixTypes = JArray
            .Parse(ModUtils.LoadConfig("Meta/AffixType.json"))
            .ToObject<List<ModAffixDataAffixType>>();
        CreateAvatarSeidMetas = JObject
            .Parse(ModUtils.LoadConfig("Meta/CreateAvatarSeidMeta.json"))
            .ToObject<Dictionary<int, ModSeidMeta>>();
        CreateAvatarDataTalentTypes = JArray
            .Parse(ModUtils.LoadConfig("Meta/CreateAvatarTalentType.json"))
            .ToObject<List<ModCreateAvatarDataTalentType>>();
        CreateAvatarDataLevelTypes = JArray
            .Parse(ModUtils.LoadConfig("Meta/CreateAvatarLevelType.json"))
            .ToObject<List<ModCreateAvatarDataLevelType>>();
        BuffSeidMetas = JObject
            .Parse(ModUtils.LoadConfig("Meta/BuffSeidMeta.json"))
            .ToObject<Dictionary<int, ModSeidMeta>>();
        BuffDataBuffTypes = JArray
            .Parse(ModUtils.LoadConfig("Meta/BuffType.json"))
            .ToObject<List<ModBuffDataBuffType>>();
        BuffDataTriggerTypes = JArray
            .Parse(ModUtils.LoadConfig("Meta/BuffTriggerType.json"))
            .ToObject<List<ModBuffDataTriggerType>>();
        BuffDataRemoveTriggerTypes = JArray
            .Parse(ModUtils.LoadConfig("Meta/BuffRemoveTriggerType.json"))
            .ToObject<List<ModBuffDataRemoveTriggerType>>();
        BuffDataOverlayTypes = JArray
            .Parse(ModUtils.LoadConfig("Meta/BuffOverlayType.json"))
            .ToObject<List<ModBuffDataOverlayType>>();
        
        DefaultAffixData = ModAffixData.Load(ModUtils.GetConfigPath("Data"))
            .ToDictionary(pair => pair.Value.ID, pair => pair.Value);
        DefaultBuffData = ModBuffData.Load(ModUtils.GetConfigPath("Data"))
            .ToDictionary(item => item.ID);
    }

    public ModProject OpenProject()
    {
        // Open file
        var paths = StandaloneFileBrowser.OpenFolderPanel("打开Mod目录", "", false);

        if (paths.Length != 0)
        {
            var path = paths[0];
            var project = ModProject.Load(path);
            return project;
        }

        return null;
    }

    public void SaveProject(ModProject modProject,string path = null)
    {
        path ??= modProject.ProjectPath;
        ModProject.Save(path, modProject);
    }
}