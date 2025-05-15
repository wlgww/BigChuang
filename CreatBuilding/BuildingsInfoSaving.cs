using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public static class BuildingsInfoSaving
{
    private const  string ThefileName = "BuildingsInfo5.json";
    // 保存数据
    public static void Save<T>(List<T> data, string fileName=ThefileName)
    {
        var wrapper = new Wrapper<T> { data = data };
        string json = JsonUtility.ToJson(wrapper, prettyPrint: true);
        Debug.Log("保存的JSON内容: " + json); // 调试输出
        File.WriteAllText(GetPath(fileName), json);
    }

    // 加载数据
    public static List<T> Load<T>(string fileName=ThefileName)
    {
        string path = GetPath(fileName);
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            Debug.Log("读取的JSON内容: " + json); // 调试输出
            return JsonUtility.FromJson<Wrapper<T>>(json).data;
        }
        return new List<T>();
    }

    private static string GetPath(string fileName)
    {
        return Path.Combine(Application.persistentDataPath, fileName);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public List<T> data;
    }
}
