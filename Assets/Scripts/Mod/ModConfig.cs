using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class ModConfig
{
    public string Name { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public static ModConfig Load(string dir)
    {
        ModConfig modConfig = null;
        string filePath = $"{dir}/modConfig.json";
        if (File.Exists(filePath))
        {
            modConfig = JObject.Parse(File.ReadAllText(filePath)).ToObject<ModConfig>();
        }

        modConfig = modConfig ?? new ModConfig();

        return modConfig;
    }
    
    public static void Save(string dir,ModConfig modConfig)
    {
        try
        {
            string filePath = $"{dir}/modConfig.json";

            var json = JObject.FromObject(modConfig).ToString(Formatting.Indented);
            File.WriteAllText(filePath, json);
        }
        catch (Exception e)
        {
            throw;
        }
    }
}