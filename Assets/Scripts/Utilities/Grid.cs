using UnityEngine;
using System.Collections.Generic;

public class Grid : MonoBehaviour
{
    [SerializeField] private GameObject gridTilePrefab;
    [SerializeField] private GameObject[] obstaclePrefabs;
    public Dictionary<Vector2Int, IGridTile> gridDictionary = new Dictionary<Vector2Int, IGridTile>();   
    
    // OnGridGenerated;
    
    private void Start()
    {
        GameData.gameData.CalculateGenerateStartPoint();
        GenerateGrid();        
    }

    private void GenerateGrid()
    {
        if (gridTilePrefab.TryGetComponent<IGridTile>(out IGridTile gridTile))
        {
            gridTile.GenerateGridTile();          
        }
        else
        {
            Debug.LogError(gridTile + "does not have IGridTile interface");
        }
    }   

    public void AddToGridDictionary(Vector2Int address, IGridTile gridTile) // called when 
    {
        gridDictionary.Add(address, gridTile);
    }

    private void GenerateConsumable()
    {
        // upon receiving an event from game manager, here will be generated spawnable object in a grid
        // will check for empty grid tile in GridDictionary
    }

    private void GenerateObstacle()
    {
        // upon receiving an event from game manager, here will be generated obstacle in a grid

        gridDictionary.TryGetValue(GetRandomGridAddress(), out IGridTile gridTileForObstacleSpawn);

        if (gridTileForObstacleSpawn.HasObject())
        {
            Debug.Log(gridTileForObstacleSpawn + "has object");            
        } else
        {
            Debug.Log(gridTileForObstacleSpawn + "doesn't have object");
            gridTileForObstacleSpawn.GenerateObstacle(obstaclePrefabs[0]);
        }
    }

    private Vector2Int GetRandomGridAddress()
    {
        int gridTileX = Random.Range(0, GameData.gameData.levelWidth);
        int gridTileY = Random.Range(0, GameData.gameData.levelHeight);
        Vector2Int randomGridAddress;

        return randomGridAddress = (new Vector2Int(gridTileX, gridTileY));
    }
}
