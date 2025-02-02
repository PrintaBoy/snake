using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private GameObject tile;
    
    private void Start()
    {
        GenerateLevel();
    }

    private void GenerateLevel()
    {
        float gridSize = GameData.gameData.gridSize;   
        float levelWidthCenter = GameData.gameData.levelWidth / 2f;
        float levelHeightCenter = GameData.gameData.levelHeight / 2f;

        for (int i = 0; i < GameData.gameData.levelWidth; i++)
        {            
            for (int j = 0; j < GameData.gameData.levelHeight; j++)
            {
                Instantiate(tile, new Vector3(((gridSize * i) - levelWidthCenter) + 0.5f, 0, ((gridSize * j) - levelHeightCenter) + 0.5f), tile.transform.rotation);                                
            }
        }
    }
}
