using UnityEngine;
using System.Collections.Generic;
using System;

public class GridController : MonoBehaviour, IGridController
{
    /// <summary>
    /// Generates and maintains grid in snake level
    /// </summary>
    
    public static GridController instance; // Singleton

    public GameObject gridParent; // keeps reference of game object under which all grid tiles are spawned
    [SerializeField] private GameObject gridTilePrefab;    
    public Dictionary<Vector2Int, IGridTile> gridDictionary = new Dictionary<Vector2Int, IGridTile>(); // keeps all grid tiles and it's addresses in dictionary

    public static event Action OnGridGenerated; // invokes when all grid tiles are generated
    public static event Action OnGridMapGenerated; // invokes when all grid tiles have created map of adjecent grid tiles

    private int gridTilesMapReady; // counts the amount of tiles with map of adjecent tiles already created

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

    private void GenerateGrid() // creates whole grid for SnakeLevel
    {   
        GameData gDataRef = GameData.gameData; // here to make code easier to read
        for (int i = 0; i < gDataRef.levelWidth; i++)
        {
            for (int j = 0; j < gDataRef.levelHeight; j++)
            {
                GameObject generatedTile = Instantiate(gridTilePrefab, new Vector3((gDataRef.generateLevelStartPoint.x) + (gDataRef.gridSize * i), 0, (gDataRef.generateLevelStartPoint.z) + (gDataRef.gridSize * j)), gameObject.transform.rotation);
                generatedTile.GetComponent<IGridTile>().SetupGridTile(new Vector2Int(i, j));
            }
        }
    }   

    public void AddToGridDictionary(Vector2Int address, IGridTile gridTile) 
    {
        /// <summary>
        /// When called this method will grid tile into gridDictionary with grid tile address
        /// </summary>

        gridDictionary.Add(address, gridTile);

        if (gridDictionary.Count >= GameData.gameData.levelWidth * GameData.gameData.levelHeight) // checks if every generated GridTile is in dictionary
        {            
            OnGridGenerated?.Invoke();            
        }
    }

    public void AdjecentTilesMapGenerated()
    {
        /// <summary>
        /// Called when grid tile creates it's map of adjecent tiles
        /// When all tiles created their maps it will invoke an event
        /// </summary>

        gridTilesMapReady++;
        if (gridTilesMapReady >= GameData.gameData.levelWidth * GameData.gameData.levelHeight)
        {
            OnGridMapGenerated?.Invoke();         
        }
    }

    public IGridTile GetTileOnAddress(Vector2Int address)
    {
        /// <summary>
        /// Returns grid tile on given address        
        /// </summary>
        return gridDictionary[address];
    }

    public IGridTile GetRandomEmptyTile() // returns empty grid tile at random position
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

    public IGridTile GetEmptyTileOutsideSafeZone(int safeZoneRadius) 
    {
        /// <summary>
        /// returns empty grid tile that is outside of snake head safe zone (safe zone radius is given in parameter)
        /// this is to prevent spawning obstacles and consumables right in front of snake head making collision unavoidable and unfair    
        /// </summary>

        IGridTile emptyTileOutsideSafeZone = null;
        bool emptyTileOutsideSafeZoneFound = false;
        IGridTile possiblyEmptyTile;

        List<IGridTile> safeZone = CreateSafeZone(SnakeController.instance.GetSnakeHeadTile(), safeZoneRadius);

        while (!emptyTileOutsideSafeZoneFound)
        {
            possiblyEmptyTile = GetRandomEmptyTile();

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

        Vector2Int randomGridAddress = (new Vector2Int(gridTileX, gridTileY));

        return randomGridAddress;
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
