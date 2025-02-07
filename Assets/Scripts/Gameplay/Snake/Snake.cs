using UnityEngine;
using System;

public class Snake : MonoBehaviour, ISpawnable
{
    private float movementSpeed = 0f;
    private float doMoveTimer = 0f;
    private float doMoveTimerMax = 0.3f; // refactor to JSON, load from gamedata

    public static event Action<Snake> OnCanMove;

    private void Awake()
    {
        movementSpeed = GameData.gameData.snakeMovementSpeed;
    }

    private void Update()
    {
        doMoveTimer += Time.deltaTime * movementSpeed;
        if (doMoveTimer >= doMoveTimerMax)
        {            
            OnCanMove?.Invoke(this);            
            doMoveTimer = 0f;
        }
    }

    public Directions GetMovementDirection()
    {        
        return Directions.North;
    }

    public void DoMovement(Transform gridToMoveTo)
    {
        gameObject.transform.position = gridToMoveTo.position;
        Debug.Log("Do movement to " + gridToMoveTo);
    }

    public void Spawn()
    {

    }

    public void Collision()
    {

    }
}
