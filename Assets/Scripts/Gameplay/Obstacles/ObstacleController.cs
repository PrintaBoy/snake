using UnityEngine;
using System.Collections.Generic;

public class ObstacleController : MonoBehaviour
{
    public static ObstacleController instance;
    private int gameTickCounter;

     public List<Rock> obstacles;
    [SerializeField] private ObjectPool rockObjectPool;

    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        TickController.OnGameTick += GameTick;
        Rock.OnObstacleDespawn += ObstacleDespawn;
        SnakeController.OnSnakeSpawned += SnakeSpawned;
    }

    private void OnDisable()
    {
        TickController.OnGameTick -= GameTick;
        Rock.OnObstacleDespawn -= ObstacleDespawn;
        SnakeController.OnSnakeSpawned -= SnakeSpawned;
    }

    private void ObstacleDespawn(Rock obstacle)
    {
        obstacles.Remove(obstacle);
    }

    private void SnakeSpawned()
    {        
        if (!SceneController.isNewGame)
        {
            for (int i = 0; i < GameData.gameData.rockObstaclesAmount; i++)
            {
                SpawnObstacle(GridController.instance.gridDictionary[GameData.gameData.rockObstaclesAddresses[i]]);
            }
            // load saved obstacles
        }
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
            SpawnObstacle(GridController.instance.GetEmptyTileOutsideSafeZone(3));
            gameTickCounter = 0;
        }
    }

    private void SpawnObstacle(IGridTile spawnTile)
    {        
        GameObject spawnedObstacle = rockObjectPool.GetPooledObject();
        spawnedObstacle.SetActive(true);
        SetupObstacle(spawnedObstacle, spawnTile);
    }

    private void SetupObstacle(GameObject spawnedObstacle, IGridTile spawnedObstacleTile)
    {
        if (spawnedObstacle.TryGetComponent<ISpawnable>(out ISpawnable spawnable)) // setup spawned consumable
        {
            spawnable.SetupSpawnable(spawnedObstacleTile);
        }

        if (spawnedObstacle.TryGetComponent<Rock>(out Rock obstacle)) // add generated Obstacle to list
        {
            obstacles.Add(obstacle);
        }
    }
}
