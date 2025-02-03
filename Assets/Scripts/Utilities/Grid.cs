using UnityEngine;
using System.Collections.Generic;

public class Grid : MonoBehaviour
{
    [SerializeField] private GameObject gridTilePrefab;
    [SerializeField] private GameObject[] obstaclePrefabs;
    public Dictionary<Vector2Int, IGridTile> gridDictionary = new Dictionary<Vector2Int, IGridTile>();    
    
    private void Start()
    {
        GameData.gameData.CalculateGenerateStartPoint();
        GenerateGrid();
        GenerateGridBorders();        
    }

    private void GenerateGrid()
    {
        if (gridTilePrefab.TryGetComponent<IGridTile>(out IGridTile gridTile))
        {
            gridTile.Generate();
        }
        else
        {
            Debug.LogError(gridTile + "does not have IGridTile interface");
        }
    }   

    private void GenerateGridBorders()
    {
        foreach (GameObject obstaclePrefab in obstaclePrefabs)
        {
            if (obstaclePrefab.TryGetComponent<IObstacle>(out IObstacle obstacle))
            {
                obstacle.GenerateGridBorders();
            }
            else
            {
                Debug.LogError(obstacle + "does not have IObstacle interface");
            }
        }
    }

    public void AddToGridDictionary(Vector2Int address, IGridTile gridTile) // called when 
    {
        gridDictionary.Add(address, gridTile);
    }

    private void GenerateSpawnable()
    {
        // upon receiving an event from game manager, here will be generated spawnable object in a grid
        // will check for empty grid tile in GridDictionary
    }

    private void GenerateObstacle()
    {
        // upon receiving an event from game manager, here will be generated obstacle in a grid
        // will check for empty grid tile in GridDictionary
    }
}
