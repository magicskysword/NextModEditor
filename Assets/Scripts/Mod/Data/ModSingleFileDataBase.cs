using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

public abstract class ModSingleFileDataBase<T> : IModData where T : ModSingleFileDataBase<T>
{
    public abstract int ID { get; set; }
    public static string FileName { get; set; }
    
    public static Dictionary<string, T> Load(string dir)
    {
        Dictionary<string, T> dataDic = null;
        string filePath = $"{dir}/{FileName}";
        if (File.Exists(filePath))
        {
            dataDic = JObject.Parse(File.ReadAllText(filePath)).ToObject<Dictionary<string, T>>();
        }

        dataDic ??= new Dictionary<string, T>();

        return dataDic;
    }

    public static void Save(string dir, Dictionary<string,T> dataDic)
    {
        string filePath = $"{dir}/{FileName}";
        if(dataDic != null)
        {
            var json = JObject.FromObject(dataDic).ToString(Formatting.Indented);
            File.WriteAllText(filePath, json);
        }
        else
        {
            File.Delete(filePath);
        }
    }
}