using UnityEngine;
using System.IO;

public class JSONParser : MonoBehaviour
{
    private string jsonGameData;
    private string jsonDataPath = Application.dataPath + "/Scripts/Utilities/GameData.json";

    private void Start()
    {
        LoadData();
    }

    public void SaveData()
    {
        jsonGameData = JsonUtility.ToJson(GameData.gameData);
        File.WriteAllText(jsonDataPath, jsonGameData);
    }

    private void LoadData()
    {
        jsonGameData = File.ReadAllText(jsonDataPath);
        GameData.gameData = JsonUtility.FromJson<GameData>(jsonGameData);
    }
}
