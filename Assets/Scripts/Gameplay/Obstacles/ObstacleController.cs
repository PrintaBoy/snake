using UnityEngine;
using System.Collections.Generic;

public class ObstacleController : MonoBehaviour
{
    private int gameTickCounter;

    [SerializeField] private List<Rock> obstacles;    
    [SerializeField] private ObjectPool rockObjectPool;

    private void OnEnable()
    {
        TickController.OnGameTick += GameTick;
        Rock.OnObstacleDespawn += ObstacleDespawn;
    }

    private void OnDisable()
    {
        TickController.OnGameTick -= GameTick;
        Rock.OnObstacleDespawn -= ObstacleDespawn;
    }

    private void ObstacleDespawn(Rock obstacle)
    {
        obstacles.Remove(obstacle);
    }

    private void GameTick()
    {
        gameTickCounter++;
        CheckObstacleSpawnCondition();
    }

    private void CheckObstacleSpawnCondition()
    {
        if (gameTickCounter >= GameData.gameData.rockSpawnRate && obstacles.Count < GameData.gameData.rockMaxSpawnCount)
        {
            SpawnObstacle();
            gameTickCounter = 0;
        }
    }

    private void SpawnObstacle()
    {        
        GameObject spawnedObstacle = rockObjectPool.GetPooledObject();
        spawnedObstacle.SetActive(true);
        SetupObstacle(spawnedObstacle);
    }

    private void SetupObstacle(GameObject spawnedObstacle)
    {
        if (spawnedObstacle.TryGetComponent<ISpawnable>(out ISpawnable spawnable)) // setup spawned consumable
        {
            spawnable.SetupSpawnable(GridController.instance.GetEmptyTileOutsideSafeZone(3));
        }

        if (spawnedObstacle.TryGetComponent<Rock>(out Rock obstacle)) // add generated Obstacle to list
        {
            obstacles.Add(obstacle);
        }
    }
}
