using UnityEngine;
using System;

public class Obstacle : MonoBehaviour, ISpawnable
{
    private IGridTile parent;
    private int gameTicksSinceSpawn = 0; // keeps track of gameTicks that have passed since spawn

    public static event Action OnObstacleCollision;
    public static event Action<Obstacle> OnObstacleDespawn;

    private void OnEnable()
    {
        SnakeController.OnSnakeCollision += Collision;
        TickController.OnGameTick += GameTick;
    }

    private void OnDisable()
    {
        SnakeController.OnSnakeCollision -= Collision;
        TickController.OnGameTick -= GameTick;
    }

    private void GameTick()
    {
        gameTicksSinceSpawn++;
        if (gameTicksSinceSpawn >= GameData.gameData.obstacleSpawnDuration)
        {
            OnObstacleDespawn?.Invoke(this);
            Destroy(gameObject);
        }
    }

    public void SetupSpawnable(IGridTile parentTile)
    {
        parent = parentTile;
        parent.BecomeParent(this);
        gameObject.transform.position = parentTile.gameObject.transform.position;
        gameObject.transform.rotation = parentTile.gameObject.transform.rotation;
    }

    public void Collision(ISpawnable collisionObject)
    {
        if (collisionObject == this)
        {
            OnObstacleCollision?.Invoke();
        }
    }

    public void ParentToTile(GridTile obstacleParentTile)
    {
        parent = obstacleParentTile;
    }
}
