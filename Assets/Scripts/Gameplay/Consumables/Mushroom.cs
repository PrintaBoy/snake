using UnityEngine;

public class Mushroom : Consumable, ISpawnable
{
    /// <summary>
    /// Mushroom reverts the movement of a snake
    /// </summary>

    private int gameTicksSinceSpawn = 0;

    private void Awake()
    {
        scoreValue = GameData.gameData.mushroomScoreValue;
    }

    public override void OnEnable()
    {
        base.OnEnable();
        TickController.OnGameTick += GameTick;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        TickController.OnGameTick -= GameTick;        
    }

    public void SetupSpawnable(IGridTile parentTile)
    {
        parent = parentTile;
        parent.BecomeParent(this);
        gameObject.transform.position = parentTile.gameObject.transform.position;
        gameObject.transform.rotation = parentTile.gameObject.transform.rotation;
    }

    public override Vector2Int GetParentGridAddress()
    {
        return parent.GetGridTileAddress();
    }

    private void GameTick()
    {
        gameTicksSinceSpawn++;

        if (gameTicksSinceSpawn >= GameData.gameData.mushroomSpawnDuration) // checks if it's time to despawn a mushroom
        {
            parent.ClearChild();
            DespawnConsumable();
        }
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
        gameTicksSinceSpawn = 0;
    }
}
