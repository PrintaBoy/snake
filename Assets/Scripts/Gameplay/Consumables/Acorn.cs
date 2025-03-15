using UnityEngine;
using System;

public class Acorn : Consumable, ISpawnable
{
    /// <summary>
    /// Acorn slowns down snake considerably
    /// </summary>

    public static event Action<Acorn> OnAcornConsumed;
    public static event Action<Acorn> OnAcornDespawn;
    private int gameTicksSinceSpawn = 0;
        
    [HideInInspector] public float snakeSpeedChange { get; private set; }

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

    private void Awake()
    {
        snakeSpeedChange = GameData.gameData.acornSnakeSpeedChange;
        scoreValue = GameData.gameData.acornScoreValue;
    }

    public Vector2Int GetParentGridAddress()
    {
        return parent.GetGridTileAddress();
    }

    private void GameTick()
    {
        gameTicksSinceSpawn++;

        if (gameTicksSinceSpawn >= GameData.gameData.acornSpawnDuration)
        {
            OnAcornDespawn?.Invoke(this);
            parent.ClearChild();
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
            OnAcornConsumed?.Invoke(this);
            DespawnConsumable();
        }
    }

    public void ParentToTile(GridTile acornParentTile)
    {
        parent = acornParentTile;
    }

    public override void DespawnConsumable()
    {        
        base.DespawnConsumable();
        gameTicksSinceSpawn = 0;
    }
}
