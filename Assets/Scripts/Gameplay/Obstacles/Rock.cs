using UnityEngine;
using System;

public class Rock : MonoBehaviour, ISpawnable
{
    private IGridTile parent;
    private int gameTicksSinceSpawn = 0; // keeps track of gameTicks that have passed since spawn

    [SerializeField] private GameObject groundStateVisual; 
    [SerializeField] private GameObject raisedStateVisual;

    private bool isRaised = false;

    public static event Action OnObstacleCollision;
    public static event Action<Rock> OnObstacleDespawn;

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

        if (gameTicksSinceSpawn >= GameData.gameData.rockGroundStateDuration)
        {
            isRaised = true;
            ToggleRockStateVisual();
        }

        if (gameTicksSinceSpawn >= GameData.gameData.rockSpawnDuration)
        {
            DestroyRock();
        }
    }

    private void ToggleRockStateVisual()
    {
        if (isRaised)
        {
            groundStateVisual.SetActive(false);
            raisedStateVisual.SetActive(true);
        } else
        {
            groundStateVisual.SetActive(true);
            raisedStateVisual.SetActive(false);
        }
    }

    public void SetupSpawnable(IGridTile parentTile)
    {
        parent = parentTile;
        parent.BecomeParent(this);
        gameObject.transform.position = parentTile.gameObject.transform.position;
        gameObject.transform.rotation = parentTile.gameObject.transform.rotation;
        isRaised = false;
        ToggleRockStateVisual();
    }

    public void Collision(ISpawnable collisionObject)
    {
        if (collisionObject == this)
        {
            if (isRaised)
            {
                OnObstacleCollision?.Invoke();
            } else
            {
                DestroyRock();
            }
            
        }
    }

    public void ParentToTile(GridTile obstacleParentTile)
    {
        parent = obstacleParentTile;
    }

    private void DestroyRock()
    {
        OnObstacleDespawn?.Invoke(this);
        Destroy(gameObject);
    }
}
