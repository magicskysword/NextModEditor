using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class ModSeidDataGroup
{
    public ModSeidMeta MetaData { get; set; }
    public List<ModSeidData> DataList { get; set; } = new List<ModSeidData>();

    public ModSeidDataGroup(ModSeidMeta meta)
    {
        MetaData = meta;
    }
    
    public static ModSeidDataGroup Load(string dir,ModSeidMeta meta)
    {
        ModSeidDataGroup data = null;
        try
        {
            string filePath = $"{dir}/{meta.ID}.json";
            if (File.Exists(filePath))
            {
                data = new ModSeidDataGroup(meta);
                var jsonData = JObject.Parse(File.ReadAllText(filePath));
                foreach (var property in jsonData.Properties())
                {
                    try
                    {
                        var seidData = ModSeidData.LoadSeidData(meta,(JObject)property.Value);
                        data.DataList.Add(seidData);
                    }
                    catch (Exception e)
                    {
                        throw new JsonException($"Seid Json {filePath} {property.Name} 读取失败", e);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }

        data ??= new ModSeidDataGroup(meta);

        return data;
    }

    public static void Save(string dir, ModSeidDataGroup dataGroup)
    {
        try
        {
            string filePath = $"{dir}/{dataGroup.MetaData.ID}.json";
            var jObject = new JObject();
            foreach (var seidData in dataGroup.DataList)
            {
                var jsonData = ModSeidData.SaveSeidData(dataGroup.MetaData, seidData);
                jObject.Add(seidData.ID.ToString(),jsonData);
            }

            File.WriteAllText(filePath, jObject.ToString(Formatting.Indented));
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }
}