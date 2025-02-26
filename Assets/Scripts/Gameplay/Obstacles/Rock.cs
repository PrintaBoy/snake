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
            DespawnRock();
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
                DespawnRock();
            }
            
        }
    }

    public void ParentToTile(GridTile obstacleParentTile)
    {
        parent = obstacleParentTile;
    }

    private void DespawnRock()
    {   
        // resets rock to previous state and let's parent gridTile to delete reference to this rock
        gameTicksSinceSpawn = 0;
        isRaised = false;
        parent.ClearChild();
        parent = null;

        OnObstacleDespawn?.Invoke(this);
        gameObject.SetActive(false);
    }
}
