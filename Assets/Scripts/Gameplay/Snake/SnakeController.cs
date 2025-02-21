using UnityEngine;
using System.Collections.Generic;
using System;

public class SnakeController : MonoBehaviour
{
    [SerializeField] private List<SnakeSegment> snakeSegments;
    [SerializeField] private GameObject snakeSegmentPrefab;

    private int startSnakeSegments;

    private Directions lastCommandDirection = Directions.West;

    public static event Action OnSnakeSpawned;    
    public static event Action<ISpawnable> OnSnakeCollision;
    public static event Action OnValidMove;

    private void Start()
    {
        startSnakeSegments = GameData.gameData.startSnakeLength;        
    }

    private void OnEnable()
    {
        GridController.OnGridMapGenerated += GridMapGenerated;
        Apple.OnAppleConsumed += AppleConsumed;
        TickController.OnSnakeTick += SnakeTick;
    }

    private void OnDisable()
    {
        GridController.OnGridMapGenerated -= GridMapGenerated;
        Apple.OnAppleConsumed -= AppleConsumed;
        TickController.OnSnakeTick -= SnakeTick;
    }

    private void AppleConsumed()
    {
        ModifySnakeSegmentAmount(1);
    }

    private void GridMapGenerated()
    {
        GenerateSnake(startSnakeSegments); // generates new snake
    }

    private void SnakeTick()
    {
        MoveSnake(lastCommandDirection);
    }

    public void ChangeSnakeDirection(Directions newDirection) // receives from MoveCommand command to change direction of snake
    {
        if (lastCommandDirection == newDirection) // snake cannot reverse into itself
        {
            return;
        }

        // this check will happen only on start when game is waiting for player input to start moving the snake
        // also this check prevents the snake to move faster in one direction by repeatedly sending command
        if (GameStateController.gameState != GameStates.Start && lastCommandDirection == Direction.GetOppositeDirection(newDirection)) 
        {
            return;
        }

        if (GameStateController.gameState == GameStates.GameOver) // snake cannot move when game is over
        {
            return;
        }

        OnValidMove?.Invoke();
        MoveSnake(newDirection);
    }

    public void MoveSnake(Directions moveDirection) // moves the snake regardless if it's player or timer input
    {    
        if (GameStateController.gameState == GameStates.GameOver || GameStateController.gameState == GameStates.Start)
        {
            return;
        }

        IGridTile adjecentTileInDirection = snakeSegments[0].GetParent().GetAdjecentTile(moveDirection);

        lastCommandDirection = moveDirection;         

        CheckForCollision(adjecentTileInDirection);
        
        snakeSegments[0].MoveSnakeSegment(adjecentTileInDirection);

        for (int i = 1; i < snakeSegments.Count; i++)
        {            
            snakeSegments[i].MoveSnakeSegment(snakeSegments[i - 1].GetPreviousParent());            
        }
    }

    private void InstantiateSnakeSegment(IGridTile tileForSnakeSegment)
    {
        GameObject generatedSnakeSegment = Instantiate(snakeSegmentPrefab);

        if (generatedSnakeSegment.TryGetComponent<SnakeSegment>(out SnakeSegment snakeSegment)) // add generated SnakeSegment to list
        {
            snakeSegments.Add(snakeSegment);
            snakeSegment.SetListIndex(snakeSegments.Count - 1);
        }

        if (generatedSnakeSegment.TryGetComponent<ISpawnable>(out ISpawnable spawnable)) //setup spawned snake segment
        {
            spawnable.SetupSpawnable(tileForSnakeSegment);
        }
    }

    private void GenerateSnake(int segmentAmount) // called on start of the level, spawns new snake
    {
        for (int i = 0; i < segmentAmount; i++)
        {
            IGridTile emptyTile;

            if (snakeSegments.Count == 0) // gets tile for spawning head
            {
                emptyTile = GridController.instance.GetEmptyTile();
                OnSnakeSpawned?.Invoke();
            }
            else // gets tile for spawning snake segment
            {
                emptyTile = snakeSegments[snakeSegments.Count - 1].GetParent();
                emptyTile = emptyTile.GetAdjecentTile(lastCommandDirection);
            }

            InstantiateSnakeSegment(emptyTile);
        }
    }

    private void ModifySnakeSegmentAmount(int segmentAmount)
    {
        for (int i = 0; i < segmentAmount; i++)
        {
            IGridTile emptyTile = snakeSegments[snakeSegments.Count - 1].GetPreviousParent();
            InstantiateSnakeSegment(emptyTile);
        }
    }

    private void CheckForCollision(IGridTile tileToCheck)
    {
        OnSnakeCollision?.Invoke(tileToCheck.GetSpawnedObject());        
    }
}
