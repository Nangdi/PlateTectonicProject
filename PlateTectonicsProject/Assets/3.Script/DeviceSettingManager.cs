using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using System.IO;
using Leap;
[System.Serializable]
public class JsonData
{
    public string P1SerialNum;
    public string P2SerialNum;
}
public class DeviceSettingManager : MonoBehaviour
{
    public static DeviceSettingManager instance;
    public JsonData jsondata;
    [SerializeField]
    LeapServiceProvider provider1;
    [SerializeField]
    LeapServiceProvider provider2;
    private string filePath;
   
    private void Awake()
    {
        instance = this;
        filePath = Path.Combine(Application.persistentDataPath, "gameData.json");
        LoadData();
        Debug.Log("¡¶¿ÃΩºµ•¿Ã≈Õ∫“∑Øø»");
        provider1.SpecificSerialNumber = jsondata.P1SerialNum;
        provider2.SpecificSerialNumber = jsondata.P2SerialNum;

    }
    private void Start()
    {
     
    }

    public void SaveData()
    {
        string json = JsonUtility.ToJson(jsondata);
        File.WriteAllText(filePath, json);
    }

    public void LoadData()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            JsonUtility.FromJsonOverwrite(json, jsondata);
        }
    }
}

