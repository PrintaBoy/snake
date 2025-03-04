using UnityEngine;
using System;

public class Grape : Consumable, ISpawnable
{
    /// <summary>
    /// Grape slows down game tick for limited amount of time
    /// </summary>

    public static event Action<Grape> OnGrapeConsumed;
    public static event Action<Grape> OnGrapeDespawn;
    private int gameTicksSinceSpawn = 0;

    [HideInInspector] public float gameSpeedChange { get; private set; }
    [HideInInspector] public int gameSpeedChangeDuration { get; private set; }

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
        gameSpeedChange = GameData.gameData.grapeGameSpeedChange;
        gameSpeedChangeDuration = GameData.gameData.grapeGameSpeedChangeDuration;
        scoreValue = GameData.gameData.grapeScoreValue;
    }

    public void SetupSpawnable(IGridTile parentTile)
    {
        parent = parentTile;
        parent.BecomeParent(this);
        gameObject.transform.position = parentTile.gameObject.transform.position;
        gameObject.transform.rotation = parentTile.gameObject.transform.rotation;
    }

    private void GameTick()
    {
        gameTicksSinceSpawn++;

        if (gameTicksSinceSpawn >= GameData.gameData.grapeSpawnDuration)
        {
            OnGrapeDespawn?.Invoke(this);
            parent.ClearChild();
            DespawnConsumable();
        }
    }

    public override void Collision(ISpawnable collisionObject)
    {
        if (collisionObject == this)
        {
            OnGrapeConsumed?.Invoke(this);
            DespawnConsumable();
        }
    }

    public void ParentToTile(GridTile grapeParentTile)
    {
        parent = grapeParentTile;
    }

    public override void DespawnConsumable()
    {
        base.DespawnConsumable();
        gameTicksSinceSpawn = 0;
    }
}
