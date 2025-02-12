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

    public void MoveSnake(Directions newDirection) // receives from MoveCommand command to change direction of snake
    {        

        IGridTile adjecentTileInDirection = snakeSegments[0].GetParent().GetAdjecentTile(newDirection);

        if (adjecentTileInDirection == snakeSegments[0].GetPreviousParent()) // this prevents snake to reverse into itself
        {
            return;
        }

        lastCommandDirection = newDirection;
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
