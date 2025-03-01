using UnityEngine;
using System;

public class Pumpkin : Consumable, ISpawnable
{
    /// <summary>
    /// Pumpkin removes existing snake segments and adds score
    /// </summary>

    public static event Action<Pumpkin> OnPumpkinConsumed;
    public static event Action<Pumpkin> OnPumpkinDespawn;
    private int gameTicksSinceSpawn = 0;

    [HideInInspector] public int removeSnakeSegmentAmount;

    private void Awake()
    {
        removeSnakeSegmentAmount = -GameData.gameData.pumpkinRemoveSnakeSegmentAmount;
        scoreValue = GameData.gameData.pumpkinScoreValue;        
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

    private void GameTick()
    {
        gameTicksSinceSpawn++;

        if (gameTicksSinceSpawn >= GameData.gameData.pumpkinSpawnDuration) // checks if it's time to despawn a pumpkin
        {
            OnPumpkinDespawn?.Invoke(this);
            parent.ClearChild(); // when pumpkin is not consumed by apple the parent tile needs to clear it's child
            DespawnConsumable();
        }
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
            OnPumpkinConsumed?.Invoke(this);
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
