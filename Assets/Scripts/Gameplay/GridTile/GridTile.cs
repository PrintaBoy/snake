using UnityEngine;
using System.Collections.Generic;

public class GridTile : MonoBehaviour, IGridTile
{
    /// <summary>
    /// This class controlls spawned grid tile
    /// It keeps track of the the address of the grid tile, keeps track of what's on the grid tile and has a map of adjecent grid tile
    /// </summary>

    public Vector2Int gridAddress;
    public ISpawnable spawnedObject = null;
    private Dictionary<Directions, IGridTile> adjecentTiles = new Dictionary<Directions, IGridTile>();    

    public void SetupGridTile(Vector2Int gridTileAddress)
    {

        /// <summary>
        /// After GridTile is spawned this method sets it up
        /// </summary>        

        gridAddress = gridTileAddress;
        name = "GridTile_" + gridTileAddress.x.ToString() + "_" + gridTileAddress.y.ToString();
        GridController.instance.AddToGridDictionary(gridAddress, this);
        transform.parent = GridController.instance.gridParent.transform;
    }

    private void OnEnable()
    {
        GridController.OnGridGenerated += OnGridGenerated;        
    }

    private void OnDisable()
    {
        GridController.OnGridGenerated -= OnGridGenerated;        
    }

    private void OnGridGenerated() // waits for OnGridGenerated event to get adjecent tiles
    {
        MapAdjecentTiles();
    }

    public IGridTile GetAdjecentTile(Directions direction) 
    {
        /// <summary>
        /// Returns adjecent tile in given direction
        /// </summary>

        IGridTile adjecentTileInDirection = adjecentTiles[direction];
        return adjecentTileInDirection;
    }

    public void BecomeParent(ISpawnable child) 
    {
        /// <summary>
        /// When something is on top of the grid tile this method is called
        /// Grid tile then becomes parent of the object on top of this grid tile
        /// </summary>

        spawnedObject = child;
        spawnedObject.ParentToTile(this);
        spawnedObject.gameObject.transform.parent = gameObject.transform;
    }

    public void ClearChild() 
    {
        /// <summary>
        /// When child object leaves, this grid tile is no longer it's parent
        /// </summary>

        spawnedObject = null;
    }

    public ISpawnable GetSpawnedObject()
    {
        /// <summary>
        /// Returns child object of this GridTile
        /// </summary>

        return spawnedObject;
    }

    public bool HasObject()
    {
        /// <summary>
        /// Returns whether this GridTile has child object or not
        /// </summary>

        bool hasObject = spawnedObject == null ? false : true;
        return hasObject;
    }

    public void MapAdjecentTiles() 
    {
        /// <summary>
        /// Creates map of tiles adjecent right next to this GridTile
        /// </summary>

        int northernTileDirection = gridAddress.y + 1 == GameData.gameData.levelHeight ? 0 : gridAddress.y + 1; // if there is no adjecent tile, loop around to the opposite border
        adjecentTiles.Add(Directions.North, GridController.instance.GetTileOnAddress(new Vector2Int(gridAddress.x, northernTileDirection))); // get North adjecent tile        

        int southernTileDirection = gridAddress.y - 1 < 0 ? GameData.gameData.levelHeight - 1 : gridAddress.y - 1;
        adjecentTiles.Add(Directions.South, GridController.instance.GetTileOnAddress(new Vector2Int(gridAddress.x, southernTileDirection))); // get South adjecent tile

        int easternTileDirection = gridAddress.x + 1 == GameData.gameData.levelWidth ? 0 : gridAddress.x + 1;
        adjecentTiles.Add(Directions.East, GridController.instance.GetTileOnAddress(new Vector2Int(easternTileDirection, gridAddress.y))); // get East adjecent tile

        int westernTileDirection = gridAddress.x - 1 < 0 ? GameData.gameData.levelWidth - 1 : gridAddress.x - 1;        
        adjecentTiles.Add(Directions.West, GridController.instance.GetTileOnAddress(new Vector2Int(westernTileDirection, gridAddress.y))); // get West adjecent tile        

        GridController.instance.AdjecentTilesMapGenerated();
    }

    public Vector2Int GetGridTileAddress()
    {
        /// <summary>
        /// Returns address of this GridTile
        /// </summary>

        return gridAddress;
    }
}