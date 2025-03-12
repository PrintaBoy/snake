using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.EventSystems;

public class SnakeController : MonoBehaviour
{
    public static SnakeController instance; // Singleton

    public List<SnakeSegment> snakeSegments;
    [SerializeField] private GameObject snakeSegmentPrefab;

    private int startSnakeSegments;

    private Directions lastCommandDirection = Directions.West;

    public static event Action OnSnakeSpawned;    
    public static event Action<ISpawnable> OnSnakeCollision;
    public static event Action OnValidMove;

    public static float snakeSpeedMultiplier { get; private set; }  // this value multiplies Time.deltaTime based on player actions for snake
    private float snakeSpeedMaxMultiplier;
    private float snakeSpeedMinMultiplier;  

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        startSnakeSegments = GameData.gameData.startSnakeLength;
        snakeSpeedMultiplier = GameData.gameData.snakeSpeedMultiplier;
        snakeSpeedMaxMultiplier = GameData.gameData.snakeSpeedMaxMultiplier;
        snakeSpeedMinMultiplier = GameData.gameData.snakeSpeedMinMultiplier;
    }

    private void OnEnable()
    {
        GridController.OnGridMapGenerated += GridMapGenerated;
        Apple.OnAppleConsumed += AppleConsumed;
        Pumpkin.OnPumpkinConsumed += PumpkinConsumed;
        Mushroom.OnMushroomConsumed += MushroomConsumed;
        Acorn.OnAcornConsumed += AcornConsumed;
        TickController.OnSnakeTick += SnakeTick;
    }

    private void OnDisable()
    {
        GridController.OnGridMapGenerated -= GridMapGenerated;
        Apple.OnAppleConsumed -= AppleConsumed;
        Pumpkin.OnPumpkinConsumed -= PumpkinConsumed;
        Mushroom.OnMushroomConsumed -= MushroomConsumed;
        Acorn.OnAcornConsumed -= AcornConsumed;
        TickController.OnSnakeTick -= SnakeTick;
    }

    private void MushroomConsumed(Mushroom mushroom)
    {        
        ReverseSnake();
    }

    private void AppleConsumed(Apple apple)
    {        
        ModifySnakeSegmentAmount(apple.addSnakeSegmentAmount);
        ModifySnakeSpeedMultiplier(apple.snakeSpeedChange);
    }

    private void PumpkinConsumed (Pumpkin pumpkin)
    {
        ModifySnakeSegmentAmount(pumpkin.removeSnakeSegmentAmount);
    }

    private void AcornConsumed (Acorn acorn)
    {
        ModifySnakeSpeedMultiplier(-acorn.snakeSpeedChange);
    }

    private void GridMapGenerated()
    {
        if (SceneController.isNewGame)
        {
            GenerateNewSnake(startSnakeSegments); // generates new snake
        }
        else
        {
            LoadSavedSnake(); // load snake from save
        }
        
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

        IGridTile adjecentTileInDirection = snakeSegments[0].GetParent().GetAdjecentTile(moveDirection); // gets next tile a snake is moving into
        ISpawnable collisionObject = adjecentTileInDirection.GetSpawnedObject(); // returns collision object on next tile snake is moving into

        lastCommandDirection = moveDirection;
        snakeSegments[0].MoveSnakeSegment(adjecentTileInDirection);        

        for (int i = 1; i < snakeSegments.Count; i++)
        {            
            snakeSegments[i].MoveSnakeSegment(snakeSegments[i - 1].GetPreviousParent());            
        }

        if (collisionObject != null)
        {
            OnSnakeCollision?.Invoke(collisionObject);            
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

    private void GenerateNewSnake(int segmentAmount) // called on start of the level, spawns new snake
    {
        for (int i = 0; i < segmentAmount; i++)
        {
            IGridTile emptyTile;

            if (snakeSegments.Count == 0) // gets tile for spawning head
            {
                emptyTile = GridController.instance.GetEmptyTile();                
            }
            else // gets tile for spawning snake segment
            {
                emptyTile = snakeSegments[snakeSegments.Count - 1].GetParent();
                emptyTile = emptyTile.GetAdjecentTile(lastCommandDirection);
            }

            InstantiateSnakeSegment(emptyTile);
        }

        OnSnakeSpawned?.Invoke();
    }

    private void LoadSavedSnake()
    {
        // loads saved snake from JSON
        InstantiateSnakeSegment(GridController.instance.gridDictionary[GameData.gameData.snakeHeadAddress]); // loads head

        for (int i = 1; i < GameData.gameData.snakeSegmentsAmount; i++)
        {
            InstantiateSnakeSegment(GridController.instance.gridDictionary[GameData.gameData.snakeSegmentsAddresses[i]]); // load snake tail segments
        }

        OnSnakeSpawned?.Invoke();
    }

    private void ModifySnakeSegmentAmount(int segmentAmount)
    {
        if (segmentAmount > 0) // adds snake segment
        {
            for (int i = 0; i < segmentAmount; i++)
            {
                IGridTile emptyTile = snakeSegments[snakeSegments.Count - 1].GetPreviousParent();
                InstantiateSnakeSegment(emptyTile);
            }
        }
        else // removes snake segments
        {
            if (snakeSegments.Count > 3)
            {
                // do multiple deletions
                snakeSegments[snakeSegments.Count - 1].DeleteSnakeSegment();
                snakeSegments.RemoveAt(snakeSegments.Count - 1);
            }            
        }
    }

    private void ModifySnakeSpeedMultiplier(float amount)
    {
        snakeSpeedMultiplier += amount;
        snakeSpeedMultiplier = Mathf.Min(snakeSpeedMultiplier, snakeSpeedMaxMultiplier); // snake cannot go faster than snakeSpeedMaxMultiplier
        
        snakeSpeedMultiplier = Mathf.Max(snakeSpeedMultiplier, snakeSpeedMinMultiplier); // snake cannot go slower than snakeSpeedMinMultiplier
    }

    public IGridTile GetSnakeHeadTile()
    {
        IGridTile snakeHeadTile = snakeSegments[0].GetParent();
        return snakeHeadTile;
    }

    private void ReverseSnake()
    {       
        Directions lastSnakeSegmentLastMoveDirection = snakeSegments[snakeSegments.Count - 1].GetLastMoveDirection();

        snakeSegments.Reverse();

        for (int i = 0; i < snakeSegments.Count; i++)
        {
            snakeSegments[i].SetListIndex(i);
        }

        lastCommandDirection = lastSnakeSegmentLastMoveDirection;
    }
}
