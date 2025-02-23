using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveLoadManager
{
    public static void Save(Data data, string filename) {
        string filePath = Application.persistentDataPath + $"/{filename}.json";
        string json = JsonConvert.SerializeObject(data, Formatting.None, new JsonSerializerSettings {
            TypeNameHandling = TypeNameHandling.All,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        });
        
        File.WriteAllText(filePath, json);
    }

    public static Data Load(string filename) {
        string filePath = Application.persistentDataPath + $"/{filename}.json";

        if (!File.Exists(filePath))
            return null;

        string json = File.ReadAllText(filePath);

        Data data = JsonConvert.DeserializeObject<Data>(json, new JsonSerializerSettings {
            TypeNameHandling = TypeNameHandling.All
        });
        return data;
    }
}
