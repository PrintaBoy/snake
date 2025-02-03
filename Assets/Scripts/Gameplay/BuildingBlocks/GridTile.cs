using UnityEngine;

public class GridTile : MonoBehaviour, IGridTile
{    
    private Grid grid;
    private Vector2Int gridAddress;

    private void Start()
    {        
        grid = GameObject.Find("LevelManager").GetComponent<Grid>();
        grid.AddToGridDictionary(gridAddress, this);
    }

    public void Generate()
    {
        GameData gDataRef = GameData.gameData; // here to make code little cleaner
        for (int i = 0; i < GameData.gameData.levelWidth; i++)
        {
            for (int j = 0; j < GameData.gameData.levelHeight; j++)
            {
                GameObject generatedTile;
                generatedTile = Instantiate(gameObject, new Vector3((gDataRef.generateLevelStartPoint.x) + (gDataRef.gridSize * i), 0, (gDataRef.generateLevelStartPoint.z) + (gDataRef.gridSize * j)), gameObject.transform.rotation);
                generatedTile.name = "GridTile_" + i.ToString() + "_" + j.ToString();
                gridAddress = new Vector2Int(i, j);                                
            }
        }
    }
}