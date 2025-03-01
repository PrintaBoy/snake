using UnityEngine;
using System;

public class Mushroom : Consumable, ISpawnable
{
    /// <summary>
    /// Mushroom reverts the movement of a snake
    /// </summary>

    public static event Action<Mushroom> OnMushroomConsumed;

    private void Awake()
    {
        scoreValue = GameData.gameData.mushroomScoreValue;
    }

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
            DespawnConsumable();
        }
    }

    public void ParentToTile(GridTile appleParentTile)
    {
        parent = appleParentTile;
    }

    public override void DespawnConsumable()
    {
        OnMushroomConsumed?.Invoke(this);
        base.DespawnConsumable();
    }
}
