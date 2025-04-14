using UnityEngine;
using System.Collections.Generic;

public class ObstacleController : MonoBehaviour
{
    /// <summary>
    /// This class handles everything concerning obstacles - spawning, despawning, obstacles setup and keeps track of spawned obstacles
    /// </summary>

    public static ObstacleController instance;
    private int gameTickCounter;

    public List<IObstacle> obstacles { get; private set; }
    [SerializeField] private ObjectPool rockObjectPool;

    private void Awake()
    {
        obstacles = new List<IObstacle>();
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

    private void ObstacleDespawn(IObstacle obstacle)
    {
        obstacles.Remove(obstacle);
    }

    private void SnakeSpawned() // if loaded from save, spawn obstacles from save
    {        
        if (!SceneController.isNewGame)
        {
            for (int i = 0; i < GameData.gameData.rockObstaclesAmount; i++)
            {
                SpawnObstacle(GridController.instance.gridDictionary[GameData.gameData.rockObstaclesAddresses[i]]); // load saved obstacles
            }
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
        spawnedObstacle.GetComponent<ISpawnable>().SetupSpawnable(spawnedObstacleTile);
        obstacles.Add(spawnedObstacle.GetComponent<IObstacle>()); // add generated Obstacle to list        
    }
}
