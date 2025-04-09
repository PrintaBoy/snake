using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.EventSystems;
using UnityEngine.SocialPlatforms.Impl;

public class SnakeController : MonoBehaviour
{
    /// <summary>
    /// This class handles everything concerning snake:
    /// - keeps list of all snake segments
    /// - increases and dcereases speed and length of snake
    /// - moves snake and changes direction if neccessary
    /// - checks if snake collided with other objects
    /// - can reverse snake
    /// </summary>

    public static SnakeController instance; // Singleton

    public List<ISnakeSegment> snakeSegments = new List<ISnakeSegment>();
    [SerializeField] private GameObject snakeSegmentPrefab;

    [HideInInspector] public Directions lastCommandDirection = Directions.West;

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
        snakeSpeedMultiplier = GameData.gameData.snakeSpeedMultiplier;
        snakeSpeedMaxMultiplier = GameData.gameData.snakeSpeedMaxMultiplier;
        snakeSpeedMinMultiplier = GameData.gameData.snakeSpeedMinMultiplier;
    }

    private void OnEnable()
    {
        GridController.OnGridMapGenerated += GridMapGenerated;
        Consumable.OnConsumableConsumed += ConsummableConsumed;        
        TickController.OnSnakeTick += SnakeTick;
    }

    private void OnDisable()
    {
        GridController.OnGridMapGenerated -= GridMapGenerated;
        Consumable.OnConsumableConsumed -= ConsummableConsumed;        
        TickController.OnSnakeTick -= SnakeTick;
    }

    private void ConsummableConsumed(ConsumableTypes consumableType)
    {
        switch (consumableType)
        {
            case ConsumableTypes.Apple:
                ModifySnakeSegmentAmount(GameData.gameData.appleAddSnakeSegmentAmount);
                ModifySnakeSpeedMultiplier(GameData.gameData.appleSnakeSpeedChange);
                break;
            case ConsumableTypes.Acorn:
                ModifySnakeSpeedMultiplier(-GameData.gameData.acornSnakeSpeedChange);
                break;
            case ConsumableTypes.Pumpkin:
                ModifySnakeSegmentAmount(GameData.gameData.pumpkinRemoveSnakeSegmentAmount);
                break;
            case ConsumableTypes.Mushroom:
                ReverseSnake();
                break;
        }
    }   

    private void GridMapGenerated()
    {
        if (SceneController.isNewGame)
        {
            GenerateNewSnake(GameData.gameData.startSnakeLength); // generates new snake
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

    public void ChangeSnakeDirection(Directions newDirection) 
    {
        /// <summary>
        /// Changes movement direction of snake
        /// Currently snake movement direction can be changed only by MoveCommand 
        /// </summary>

        // this prevents snake from reversing into itself
        // it also prevents movement when game is in game over state
        if (lastCommandDirection == newDirection || GameStateController.gameState == GameStates.GameOver) // this prevents snake from reversing into itself
        {
            return;
        }

        // this check will happen only on start when game is waiting for player input to start moving the snake
        // also this check prevents the snake to move faster in one direction by repeatedly sending command
        if (GameStateController.gameState != GameStates.Start && lastCommandDirection == Direction.GetOppositeDirection(newDirection)) 
        {
            return;
        }        

        OnValidMove?.Invoke();
        MoveSnake(newDirection);
    }

    public void MoveSnake(Directions moveDirection) // moves the snake regardless if it's player or timer input
    {
        /// <summary>
        /// this method will move snake segments from current tile to next tile
        /// this method is either called by ChangeSnakeDirection (player input) or by enough time passing (tick input)
        /// </summary>

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

        snakeSegments.Add(generatedSnakeSegment.GetComponent<ISnakeSegment>());
        generatedSnakeSegment.GetComponent<ISnakeSegment>().SetListIndex(snakeSegments.Count - 1);
        
        generatedSnakeSegment.GetComponent<ISpawnable>().SetupSpawnable(tileForSnakeSegment); // setup spawned snake segment
    }

    private void GenerateNewSnake(int segmentAmount) // called on start of the level when NewGame is chosen, spawns new snake
    {
        for (int i = 0; i < segmentAmount; i++)
        {
            IGridTile emptyTile;

            if (snakeSegments.Count == 0) // gets tile for spawning head
            {
                emptyTile = GridController.instance.GetRandomEmptyTile();                
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

    private void LoadSavedSnake() // loads saved snake from JSON     
    {
        for (int i = 0; i < GameData.gameData.snakeSegmentsAmount; i++)
        {
            InstantiateSnakeSegment(GridController.instance.gridDictionary[GameData.gameData.snakeSegmentsAddresses[i]]);
        }
        
        lastCommandDirection = Direction.GetOppositeDirection(GameData.gameData.lastMoveDirection); // needs to be reverted to prevent loaded snake into reversing into itself        

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
                // TODO multiple deletions
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
        /// <summary>
        /// returns snake head parent grid tile
        /// </summary>        

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
