using UnityEngine;
using System;

public class Apple : Consumable, ISpawnable
{
    public static event Action OnAppleConsumed;

    public void SetupSpawnable(IGridTile parentTile)
    {
        parent = parentTile;
        parent.BecomeParent(this);
        gameObject.transform.position = parentTile.gameObject.transform.position;
        gameObject.transform.rotation = parentTile.gameObject.transform.rotation;
    }

    public override void Collision(ISpawnable collisionObject)
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
