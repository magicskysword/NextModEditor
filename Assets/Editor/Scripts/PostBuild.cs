using System.IO;
using UnityEngine;

public static class PostBuild
{
    [UnityEditor.Callbacks.PostProcessBuild(999)]
    public static void OnPostprocessBuild(UnityEditor.BuildTarget BuildTarget, string path)
    {
        var sourcePath = $"{Application.dataPath}/../Config";
        var targetPath = $"{Path.GetDirectoryName(path)}/Config";
        if(Directory.Exists(targetPath))
            Directory.Delete(targetPath,true);
        Directory.CreateDirectory(targetPath);
        ModUtils.CopyDirectory(sourcePath, targetPath, true);
    }
}