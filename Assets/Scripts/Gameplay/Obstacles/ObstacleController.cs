using UnityEngine;
using System.Collections.Generic;

public class ObstacleController : MonoBehaviour
{
    private int gameTickCounter;

    [SerializeField] private List<Obstacle> obstacles;
    [SerializeField] private GameObject obstaclePrefab;

    private void OnEnable()
    {
        TickController.OnGameTick += GameTick;
        Obstacle.OnObstacleDespawn += ObstacleDespawn;
    }

    private void OnDisable()
    {
        TickController.OnGameTick -= GameTick;
        Obstacle.OnObstacleDespawn -= ObstacleDespawn;
    }

    private void ObstacleDespawn(Obstacle obstacle)
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
        if (gameTickCounter >= GameData.gameData.obstacleSpawnRate && obstacles.Count < GameData.gameData.obstacleMaxSpawnCount)
        {
            SpawnObstacle();
            gameTickCounter = 0;
        }
    }

    private void SpawnObstacle()
    {
        GameObject spawnedObstacle = Instantiate(obstaclePrefab);
        SetupObstacle(spawnedObstacle);
    }

    private void SetupObstacle(GameObject spawnedObstacle)
    {
        if (spawnedObstacle.TryGetComponent<ISpawnable>(out ISpawnable spawnable)) // setup spawned consumable
        {
            spawnable.SetupSpawnable(GridController.instance.GetEmptyTile());
        }

        if (spawnedObstacle.TryGetComponent<Obstacle>(out Obstacle obstacle)) // add generated Obstacle to list
        {
            obstacles.Add(obstacle);
        }
    }
}
