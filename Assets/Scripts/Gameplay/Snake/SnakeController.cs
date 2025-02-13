using UnityEngine;
using System.Collections.Generic;
using System;

public class SnakeController : MonoBehaviour
{
    [SerializeField] private List<SnakeSegment> snakeSegments;
    [SerializeField] private GameObject snakeSegmentPrefab;

    private Directions lastCommandDirection = Directions.West;

    private float movementSpeed = 0.1f;
    private float doMoveTimer = 0f;
    private float doMoveTimerMax = 1f; // refactor to JSON, load from gamedata

    public static event Action OnSnakeSpawned;

    private void Start()
    {
        movementSpeed = GameData.gameData.snakeMovementSpeed;
    }

    private void Update()
    {
        doMoveTimer += Time.deltaTime * movementSpeed;
        if (doMoveTimer >= doMoveTimerMax)
        {
            MoveSnake(lastCommandDirection);
        }
    }

    private void OnEnable()
    {
        GridController.OnGridGenerated += GridGenerated;
        Consumable.OnAppleConsumed += GenerateSnakeSegment;
    }

    private void OnDisable()
    {
        GridController.OnGridGenerated -= GridGenerated;
        Consumable.OnAppleConsumed -= GenerateSnakeSegment;
    }

    private void GridGenerated() // reacts to event Grid.OnGridGenerated
    {
        GenerateSnakeSegment(); // spawns the first segment of a snake
    }

    public void ChangeSnakeDirection(Directions newDirection) // receives from MoveCommand command to change direction of snake
    {
        Directions oppositeDirection = Directions.East;
        switch (newDirection) // gets opposite direction of the new direction
        {
            case Directions.North:
                oppositeDirection = Directions.South;
                break;
            case Directions.South:
                oppositeDirection = Directions.North;
                break;
            case Directions.East:
                oppositeDirection = Directions.West;
                break;
            case Directions.West:
                oppositeDirection = Directions.East;
                break;
        }

        if (lastCommandDirection == newDirection || lastCommandDirection == oppositeDirection) // this check prevents the snake to reverse into itself or move faster in one direction by repeatedly sending command
        {
            return;
        } else
        {
            MoveSnake(newDirection);    
        }
    }

    public void MoveSnake(Directions moveDirection) // moves the snake regardless if it's player or timer input
    {        
        IGridTile adjecentTileInDirection = snakeSegments[0].GetParent().GetAdjecentTile(moveDirection);

        lastCommandDirection = moveDirection;
        doMoveTimer = 0f;

        snakeSegments[0].CheckForCollision(adjecentTileInDirection);        
        snakeSegments[0].MoveSnakeSegment(adjecentTileInDirection);

        for (int i = 1; i < snakeSegments.Count; i++)
        {            
            snakeSegments[i].MoveSnakeSegment(snakeSegments[i - 1].GetPreviousParent());            
        }
    }

    private void GenerateSnakeSegment()
    {
        IGridTile emptyTile;

        if (snakeSegments.Count == 0) // gets tile for spawning head
        {
            emptyTile = GridController.instance.GetEmptyTile();
            OnSnakeSpawned?.Invoke();
        }
        else // gets tile for spawning snake segment
        {            
            emptyTile = snakeSegments[snakeSegments.Count - 1].GetPreviousParent();
        }
        
        GameObject generatedSnakeSegment = Instantiate(snakeSegmentPrefab);
        
        if (generatedSnakeSegment.TryGetComponent<ISpawnable>(out ISpawnable spawnable)) //setup spawned snake segment
        {
            spawnable.SetupSpawnable(emptyTile);            
        }

        if (generatedSnakeSegment.TryGetComponent<SnakeSegment>(out SnakeSegment snakeSegment)) // add generated SnakeSegment to list
        {               
            snakeSegments.Add(snakeSegment);            
        }
    }
}
