using UnityEngine;
using System.Collections.Generic;

public class GridTile : MonoBehaviour, IGridTile
{       
    private Vector2Int gridAddress;
    public GameObject spawnedObject = null;
    private Dictionary<Directions, IGridTile> adjecentTiles = new Dictionary<Directions, IGridTile>();

    public void SetupGridTile(Vector2Int gridTileAddress)
    {
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
        GetAdjecentTiles();
    }

    public IGridTile GetAdjecentTile(Directions direction) // gets adjecent tile in given direction
    {
        IGridTile adjecentTileInDirection = adjecentTiles[direction];
        return adjecentTileInDirection;
    }

    public void BecomeParent(GameObject child) // once something spawns it calls this method to let the GridTile know it's a parent
    {
        spawnedObject = child;
        spawnedObject.GetComponent<ISpawnable>().ParentToTile(this);
        child.transform.parent = gameObject.transform;
    }

    public void ClearChild() // once something dissapears from this GridTile this clears it
    {
        spawnedObject = null;
    }

    public void GenerateGridTile() // this will generate this GridTile
    {
        GameData gDataRef = GameData.gameData; // here to make code little cleaner
        for (int i = 0; i < gDataRef.levelWidth; i++)
        {
            for (int j = 0; j < gDataRef.levelHeight; j++)
            {                
                GameObject generatedTile = Instantiate(gameObject, new Vector3((gDataRef.generateLevelStartPoint.x) + (gDataRef.gridSize * i), 0, (gDataRef.generateLevelStartPoint.z) + (gDataRef.gridSize * j)), gameObject.transform.rotation);                
                generatedTile.GetComponent<GridTile>().SetupGridTile(new Vector2Int(i, j));                
            }
        }
    }

    public GameObject GetSpawnedObject() // refactor to return ISpawnable
    {
        return spawnedObject;
    }

    public bool HasObject()
    {
        bool hasObject = spawnedObject == null ? false : true;
        return hasObject;
    }

    public void GetAdjecentTiles() 
    {
        int northernTileDirection = gridAddress.y + 1 == GameData.gameData.levelHeight ? 0 : gridAddress.y + 1; // if there is no adjecent tile, loop around to the opposite border
        adjecentTiles.Add(Directions.North, GridController.instance.GetTile(new Vector2Int(gridAddress.x, northernTileDirection))); // get North adjecent tile        

        int southernTileDirection = gridAddress.y - 1 < 0 ? GameData.gameData.levelHeight - 1 : gridAddress.y - 1;
        adjecentTiles.Add(Directions.South, GridController.instance.GetTile(new Vector2Int(gridAddress.x, southernTileDirection))); // get South adjecent tile

        int easternTileDirection = gridAddress.x + 1 == GameData.gameData.levelWidth ? 0 : gridAddress.x + 1;
        adjecentTiles.Add(Directions.East, GridController.instance.GetTile(new Vector2Int(easternTileDirection, gridAddress.y))); // get East adjecent tile

        int westernTileDirection = gridAddress.x - 1 < 0 ? GameData.gameData.levelWidth - 1 : gridAddress.x - 1;        
        adjecentTiles.Add(Directions.West, GridController.instance.GetTile(new Vector2Int(westernTileDirection, gridAddress.y))); // get West adjecent tile        
    }

    public void GenerateObstacle(GameObject obstacleToGenerate) // refactor to ObstacleController once setup
    {
        spawnedObject = Instantiate(obstacleToGenerate, transform.position, transform.rotation, transform);
    }
}