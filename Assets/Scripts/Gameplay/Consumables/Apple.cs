using UnityEngine;
using System;

public class Apple : Consumable, ISpawnable
{
    public static event Action<Apple> OnAppleConsumed;

    [SerializeField] private GameObject consumeParticle;
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
            DespawnApple();
        }
    }

    public void ParentToTile(GridTile appleParentTile)
    {
        parent = appleParentTile;
    }

    private void DespawnApple()
    {
        Instantiate(consumeParticle, gameObject.transform.position, gameObject.transform.rotation); // spawn consume particle
        parent = null;        
        
        OnAppleConsumed?.Invoke(this);
        gameObject.SetActive(false);
    }
}
