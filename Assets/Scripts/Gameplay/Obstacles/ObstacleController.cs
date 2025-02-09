using UnityEngine;
using System.Collections.Generic;

public class ObstacleController : MonoBehaviour
{
    [SerializeField] private List<Obstacle> obstacles;
    [SerializeField] private GameObject obstaclePrefab;

    private void GenerateObstacle()
    {
        // upon receiving an event from game manager, here will be generated obstacle in a grid
        // refactor 

        /*gridDictionary.TryGetValue(GetRandomGridAddress(), out IGridTile gridTileForObstacleSpawn);

        if (gridTileForObstacleSpawn.HasObject())
        {
            Debug.Log(gridTileForObstacleSpawn + "has object");
        }
        else
        {
            Debug.Log(gridTileForObstacleSpawn + "doesn't have object");
            gridTileForObstacleSpawn.GenerateObstacle(obstaclePrefabs[0]);
        }*/
    }
}
