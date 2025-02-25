using UnityEngine;
using System.Collections.Generic;
using System;

public class GridController : MonoBehaviour
{
    public static GridController instance; // Singleton

    public GameObject gridParent;    
    [SerializeField] private GameObject gridTilePrefab;    
    private Dictionary<Vector2Int, IGridTile> gridDictionary = new Dictionary<Vector2Int, IGridTile>(); // first value is address

    public static event Action OnGridGenerated; // invokes when all grid tiles are generated
    public static event Action OnGridMapGenerated; // invokes when all grid tiles have created map of adjecent grid tiles

    private int gridTilesMapReady; // counts the amount of tiles which has map of adjecent tiles ready

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

    public void AdjecentTilesMapGenerated()
    {
        gridTilesMapReady++;
        if (gridTilesMapReady >= GameData.gameData.levelWidth * GameData.gameData.levelHeight)
        {
            OnGridMapGenerated?.Invoke();         
        }
    }

    public IGridTile GetTile(Vector2Int address)
    {
        return gridDictionary[address];
    }

    public IGridTile GetEmptyTile() // returns empty grid tile at random position
    {        
        IGridTile emptyTile = null;
        bool emptyTileFound = false;
        while (!emptyTileFound)
        {
            gridDictionary.TryGetValue(GetRandomGridAddress(), out IGridTile possiblyEmptyTile);
            if (possiblyEmptyTile.GetSpawnedObject() == null)
            {                
                emptyTile = possiblyEmptyTile;
                emptyTileFound = true;
            }
        }        
        return emptyTile;
    }

    // returns empty grid tile that is outside of snake head safe zone
    // this is to prevent spawning obstacles and consumables right in front of snake head making collision unavoidable and unfair

    public IGridTile GetEmptyTileOutsideSafeZone(int safeZoneRadius) 
    {
        IGridTile emptyTileOutsideSafeZone = null;
        bool emptyTileOutsideSafeZoneFound = false;
        IGridTile possiblyEmptyTile;
        List<IGridTile> safeZone = CreateSafeZone(SnakeController.instance.GetSnakeHeadTile(), safeZoneRadius);

        while (!emptyTileOutsideSafeZoneFound)
        {
            possiblyEmptyTile = GetEmptyTile();

            if (!safeZone.Contains(possiblyEmptyTile))
            {
                emptyTileOutsideSafeZone = possiblyEmptyTile;
                emptyTileOutsideSafeZoneFound = true;
            }
        }

        return emptyTileOutsideSafeZone;
    }

    private Vector2Int GetRandomGridAddress()
    {
        int gridTileX = UnityEngine.Random.Range(0, GameData.gameData.levelWidth);
        int gridTileY = UnityEngine.Random.Range(0, GameData.gameData.levelHeight);
        Vector2Int randomGridAddress;

        return randomGridAddress = (new Vector2Int(gridTileX, gridTileY));
    }

    private List<IGridTile> CreateSafeZone(IGridTile origin, int radius) // create and return list of IGridTiles that create safe zone around origin in radius
    {
        List<IGridTile> safeZoneTiles = new List<IGridTile>();

        Vector2Int originAddress = origin.GetGridTileAddress();
        Vector2Int radiusMax = new Vector2Int(originAddress.x + radius, originAddress.y + radius);
        Vector2Int radiusMin = new Vector2Int(originAddress.x - radius, originAddress.y - radius);

        // TODO - take into account that radius can also (and should) loop around to other side of the grid

        foreach (Vector2Int gridTileAddress in gridDictionary.Keys) // checks if given gridDictionary address is within safe zone radius
        {
            if (gridTileAddress.x >= radiusMin.x && gridTileAddress.y >= radiusMin.y)
            {
                if(gridTileAddress.x <= radiusMax.x && gridTileAddress.y <= radiusMax.y)
                {
                    safeZoneTiles.Add(gridDictionary[gridTileAddress]); // if it is within safe zone radius, gridTile is added into safeZoneTiles
                }
            }
        }

        return safeZoneTiles;
    }
}
