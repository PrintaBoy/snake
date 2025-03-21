using UnityEngine;
using System;

public class Apple : Consumable, ISpawnable
{
    /// <summary>
    /// Apple adds snake segments, adds score and increases snake movement speed
    /// </summary>
    
    public static event Action<Apple> OnAppleConsumed;    

    [HideInInspector] public int addSnakeSegmentAmount;
    [HideInInspector] public float snakeSpeedChange;

    private void Awake()
    {
        addSnakeSegmentAmount = GameData.gameData.appleAddSnakeSegmentAmount;
        snakeSpeedChange = GameData.gameData.appleSnakeSpeedChange;
        scoreValue = GameData.gameData.appleScoreValue;
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

    public override Vector2Int GetParentGridAddress()
    {
        return parent.GetGridTileAddress();
    }

    public override void DespawnConsumable()
    {
        OnAppleConsumed?.Invoke(this);
        base.DespawnConsumable();    
    }
}
