using UnityEngine;
using System.Collections.Generic;

public class GridTile : MonoBehaviour, IGridTile
{       
    private Vector2Int gridAddress;
    [HideInInspector] public GameObject spawnedObject = null;

    private Dictionary<Directions, IGridTile> adjecentTiles = new Dictionary<Directions, IGridTile>();

    public void SetupGridTile(Vector2Int gridTileAddress)
    {
        gridAddress = gridTileAddress;
        name = "GridTile_" + gridTileAddress.x.ToString() + "_" + gridTileAddress.y.ToString();
        Grid.instance.AddToGridDictionary(gridAddress, this);
        transform.parent = Grid.instance.gridParent.transform;
    }

    private void OnEnable()
    {
        Grid.OnGridGenerated += OnGridGenerated;        
    }

    private void OnDisable()
    {
        Grid.OnGridGenerated -= OnGridGenerated;        
    }

    private void OnGridGenerated()
    {
        GetAdjecentTiles();
    }

    public IGridTile GetAdjecentTile(Directions direction)
    {
        IGridTile tileToMoveSnakeTo = adjecentTiles[direction];
        return tileToMoveSnakeTo;
    }

    public void BecomeParent(GameObject child)
    {
        spawnedObject = child;
        spawnedObject.GetComponent<Snake>().GetParent(this);
        child.transform.parent = gameObject.transform;
    }

    public void GenerateGridTile()
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

    public Transform GetGridPosition()
    {
        return transform;
    }

    public void GetAdjecentTiles()
    {
        int northernTileDirection = gridAddress.y + 1 == GameData.gameData.levelHeight ? 0 : gridAddress.y + 1; // if there is no adjecent tile, loop around to the opposite border
        adjecentTiles.Add(Directions.North, Grid.instance.GetTile(new Vector2Int(gridAddress.x, northernTileDirection))); // get North adjecent tile
        Debug.Log(gameObject.name + "My northern tile is" + adjecentTiles[Directions.North]);

        int southernTileDirection = gridAddress.y - 1 < 0 ? GameData.gameData.levelHeight - 1 : gridAddress.y - 1;
        adjecentTiles.Add(Directions.South, Grid.instance.GetTile(new Vector2Int(gridAddress.x, southernTileDirection))); // get South adjecent tile

        int easternTileDirection = gridAddress.x + 1 == GameData.gameData.levelWidth ? 0 : gridAddress.x + 1;
        adjecentTiles.Add(Directions.East, Grid.instance.GetTile(new Vector2Int(easternTileDirection, gridAddress.y))); // get East adjecent tile

        int westernTileDirection = gridAddress.x - 1 < 0 ? GameData.gameData.levelWidth - 1 : gridAddress.x - 1;        
        adjecentTiles.Add(Directions.West, Grid.instance.GetTile(new Vector2Int(westernTileDirection, gridAddress.y))); // get West adjecent tile        
    }

    public GameObject GetSpawnedObject()
    {
        return spawnedObject;
    }

    public void GenerateObstacle(GameObject obstacleToGenerate)
    {
        spawnedObject = Instantiate(obstacleToGenerate, transform.position, transform.rotation, transform);
    }

    public void GenerateSnake(GameObject snakeToGenerate)
    {
        spawnedObject = Instantiate(snakeToGenerate, transform.position, transform.rotation);
        BecomeParent(spawnedObject);
    }    

    public bool HasObject()
    {
        bool hasObject = spawnedObject == null ? false : true;
        return hasObject;
    }
}