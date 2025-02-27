using UnityEngine;
using System;

public class Pumpkin : Consumable, ISpawnable
{
    /// <summary>
    /// Pumpkin removes existing snake segments and adds score
    /// </summary>

    public static event Action<Pumpkin> OnPumpkinConsumed;

    [HideInInspector] public int removeSnakeSegmentAmount;

    private void Awake()
    {
        removeSnakeSegmentAmount = -GameData.gameData.pumpkinRemoveSnakeSegmentAmount;
        scoreValue = GameData.gameData.pumpkinScoreValue;
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
        OnPumpkinConsumed?.Invoke(this);
        base.DespawnConsumable();
    }
}
