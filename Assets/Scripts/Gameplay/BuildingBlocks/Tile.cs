using UnityEngine;

public class Tile : BuildingBlock, IGeneratable
{
    public void Generate()
    {
        GameData gDataRef = GameData.gameData; // here to make code little cleaner
        for (int i = 0; i < GameData.gameData.levelWidth; i++)
        {
            for (int j = 0; j < GameData.gameData.levelHeight; j++)
            {   
                Instantiate(gameObject, new Vector3((gDataRef.generateLevelStartPoint.x) + (gDataRef.gridSize * i), 0, (gDataRef.generateLevelStartPoint.z) + (gDataRef.gridSize * j)), gameObject.transform.rotation);
            }
        }
    }
}