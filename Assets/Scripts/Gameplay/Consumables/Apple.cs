using UnityEngine;
using System;

public class Apple : Consumable, ISpawnable
{
    public static event Action<Apple> OnAppleConsumed;

    [HideInInspector] public int addSnakeSegmentAmount;
    [HideInInspector] public float snakeSpeedChange;
    [HideInInspector] public int scoreValue;

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
            OnAppleConsumed?.Invoke(this);            
            Destroy(gameObject);
        }
    }

    public void ParentToTile(GridTile snakeParentTile)
    {
        parent = snakeParentTile;
    }
}
