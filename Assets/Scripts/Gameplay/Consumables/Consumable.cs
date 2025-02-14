using UnityEngine;
using System;

public class Consumable : MonoBehaviour, ISpawnable
{
    private IGridTile parent;
    public static event Action OnAppleConsumed;

    private void OnEnable()
    {
        SnakeController.OnSnakeCollision += Collision;
    }

    private void OnDisable()
    {
        
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
            OnAppleConsumed?.Invoke();
            Destroy(gameObject);
        }
    }

    public void ParentToTile(GridTile snakeParentTile)
    {
        parent = snakeParentTile;
    }
}
