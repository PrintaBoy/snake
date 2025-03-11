using UnityEngine;
using System.IO;

public class JSONParser : MonoBehaviour
{
    private string jsonGameData;
    private string jsonDataPath = Application.dataPath + "/Scripts/Utilities/GameData.json";

    private void Awake()
    {
        LoadData();
        GameData.gameData.OnAwake(); // might be more suited to refactor somewhere else. But for now it works from here
    }

    private void OnEnable()
    {
        GameData.OnSaveData += SaveData;
    }

    private void OnDisable()
    {
        GameData.OnSaveData -= SaveData;
    }

    public void SaveData()
    {
        jsonGameData = JsonUtility.ToJson(GameData.gameData, true);
        File.WriteAllText(jsonDataPath, jsonGameData);        
    }

    private void LoadData()
    {
        jsonGameData = File.ReadAllText(jsonDataPath);
        GameData.gameData = JsonUtility.FromJson<GameData>(jsonGameData);
    }
}
