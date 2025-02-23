using UnityEngine;
using System;

public class Obstacle : MonoBehaviour, ISpawnable
{
    private IGridTile parent;

    public static event Action OnObstacleCollision;

    public void OnEnable()
    {
        SnakeController.OnSnakeCollision += Collision;
    }

    public void OnDisable()
    {
        SnakeController.OnSnakeCollision -= Collision;
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
            Debug.Log("Collision");
        }
    }

    public void ParentToTile(GridTile obstacleParentTile)
    {
        parent = obstacleParentTile;
    }
}
