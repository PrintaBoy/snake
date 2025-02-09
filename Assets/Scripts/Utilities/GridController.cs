using UnityEngine;
using System.Collections.Generic;
using System;

public class GridController : MonoBehaviour
{
    public static GridController instance; // Singleton

    public GameObject gridParent;    
    [SerializeField] private GameObject gridTilePrefab;
    [SerializeField] private GameObject[] obstaclePrefabs;
    private Dictionary<Vector2Int, IGridTile> gridDictionary = new Dictionary<Vector2Int, IGridTile>();

    public static event Action OnGridGenerated;    

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

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

    public void AddToGridDictionary(Vector2Int address, IGridTile gridTile) 
    {
        gridDictionary.Add(address, gridTile);

        if (gridDictionary.Count >= GameData.gameData.levelWidth * GameData.gameData.levelHeight) // checks if every generated GridTile is in dictionary
        {            
            OnGridGenerated?.Invoke();            
        }
    }

    public IGridTile GetTile(Vector2Int address)
    {
        return gridDictionary[address];
    }

    private void GenerateConsumable()
    {
        // upon receiving an event from game manager, here will be generated spawnable object in a grid
        // will check for empty grid tile in GridDictionary
        // refactor to ConsumableController once setup
    }

    private void GenerateObstacle()
    {
        // upon receiving an event from game manager, here will be generated obstacle in a grid
        // refactor to ObstacleController once setup

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

    public IGridTile GetEmptyTile() // change so it looks for new grid tile if the current one has object
    {
        gridDictionary.TryGetValue(GetRandomGridAddress(), out IGridTile emptyTile);

        return emptyTile;
    }

    private Vector2Int GetRandomGridAddress()
    {
        int gridTileX = UnityEngine.Random.Range(0, GameData.gameData.levelWidth);
        int gridTileY = UnityEngine.Random.Range(0, GameData.gameData.levelHeight);
        Vector2Int randomGridAddress;

        return randomGridAddress = (new Vector2Int(gridTileX, gridTileY));
    }
}
