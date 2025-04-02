using UnityEngine;

public class Apple : Consumable, ISpawnable
{
    /// <summary>
    /// Apple adds snake segments, adds score and increases snake movement speed
    /// </summary>      

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
            base.InvokeConsumableConsumedEvent();
            DespawnConsumable();
        }
    }

    public void ParentToTile(GridTile appleParentTile)
    {
        parent = appleParentTile;
    }

    public override void DespawnConsumable()
    {             
        base.DespawnConsumable();    
    }
}
