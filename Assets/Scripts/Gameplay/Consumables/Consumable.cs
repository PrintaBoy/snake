using UnityEngine;
using System;

public class Consumable : MonoBehaviour, ISpawnable
{
    private IGridTile parent;
    public static event Action OnAppleConsumed;
    public void SetupSpawnable(IGridTile parentTile)
    {
        parent = parentTile;
        parent.BecomeParent(gameObject);
    }

    public void Collision()
    {
        OnAppleConsumed?.Invoke();        
        Destroy(gameObject);
    }

    public void ParentToTile(GridTile snakeParentTile)
    {
        parent = snakeParentTile;
    }
}
