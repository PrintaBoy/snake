using UnityEngine;
using System;

[System.Serializable]
public class GameData
{
    public static GameData gameData; // singleton

    // data for grid
    public int levelWidth;
    public int levelHeight;
    public float gridSize;
    public Vector3 generateLevelStartPoint;

    // data for Game Tick
    public float gameSpeedMultiplier;
    public float gameTickLength;

    // data for snake
    public int startSnakeLength;
    public float snakeSpeedMultiplier;
    public float snakeSpeedMaxMultiplier;
    public float snakeSpeedMinMultiplier;
    public float snakeTickLength;

    // data for Apple consumable
    public int appleAddSnakeSegmentAmount;
    public float appleSnakeSpeedChange;
    public int appleScoreValue;
    public int appleSpawnRate;
    public int appleSpawnDuration;
    public int appleMaxAmount;

    // data for Pumpkin consumable

    public int pumpkinRemoveSnakeSegmentAmount;
    public int pumpkinScoreValue;
    public int pumpkinSpawnRate;
    public int pumpkinSpawnDuration;

    // data for Mushroom consumable

    public int mushroomScoreValue;
    public int mushroomSpawnRate;
    public int mushroomSpawnDuration;

    // data for Acorn consumable

    public float acornSnakeSpeedChange;
    public int acornScoreValue;
    public int acornSpawnRate;
    public int acornSpawnDuration;

    // data for Grape consumable

    public float grapeGameSpeedChange;
    public int grapeGameSpeedChangeDuration; // how long the speed change will last, in ticks
    public int grapeScoreValue;
    public int grapeSpawnRate;
    public int grapeSpawnDuration;    

    // data for Rock obstacle
    public int rockSpawnRate; // how long before next obstacle is spawned, measured in Game Ticks
    public int rockSpawnDuration; // how long obstacle will stay on grid before it dissapears, measured in Game Ticks
    public int rockMaxSpawnCount; // how many obstacles can be on grid at the same time
    public int rockGroundStateDuration; // how long before rock is raised from the ground and becomes deadly on collision

    // saved game data
    public bool isGameSaved; // determines if there is actually a saved game to load from
    public int currentScore;
    public int highestScore;
    public int snakeSegmentsAmount;
    public Directions lastMoveDirection;
    public Vector2Int[] snakeSegmentsAddresses;
    public int rockObstaclesAmount;
    public Vector2Int[] rockObstaclesAddresses;
    public ConsumableTypes[] consumableListTypes;
    public Vector2Int[] consumableListAddresses;
    public int consumableListAmount;

    public static event Action OnSaveData;

    public void OnAwake() // called from other function because this class is non-MonoBehavior
    {
        gameData = this;
        ScoreController.OnNewHighScore += SaveNewHighScore;
        ButtonEventInvoker.OnQuitGameButtonPressed += SaveGame;
        ButtonEventInvoker.OnRestartButtonPressed += ClearSavedGame; // deletes saved data when player hits restart
    }

    private void SaveNewHighScore()
    {
        highestScore = ScoreController.scoreHighest;
        OnSaveData?.Invoke();
    }

    private void SaveSnake()
    {
        snakeSegmentsAmount = SnakeController.instance.snakeSegments.Count; // length of snake
        snakeSegmentsAddresses = new Vector2Int[snakeSegmentsAmount]; // initialize array
        lastMoveDirection = SnakeController.instance.lastCommandDirection;

        for (int i = 0; i < snakeSegmentsAmount; i++)
        {
            snakeSegmentsAddresses[i] = SnakeController.instance.snakeSegments[i].GetParentGridAddress(); // save snake segments
        }                
    }

    private void SaveObstacles()
    {
        rockObstaclesAmount = ObstacleController.instance.obstacles.Count;
        rockObstaclesAddresses = new Vector2Int[rockObstaclesAmount];
        for (int i = 0; i < rockObstaclesAmount; i++)
        {
            rockObstaclesAddresses[i] = ObstacleController.instance.obstacles[i].GetParentGridAddress();
        }
    }

    private void SaveScore()
    {
        currentScore = ScoreController.scoreCurrent;
    }

    private void SaveConsumables()
    {
        consumableListAmount = ConsumableController.instance.consumables.Count;
        consumableListAddresses = new Vector2Int[consumableListAmount];
        consumableListTypes = new ConsumableTypes[consumableListAmount];

        for (int i = 0; i < consumableListAmount; i++)
        {
            consumableListAddresses[i] = ConsumableController.instance.consumables[i].GetParentGridAddress();
            consumableListTypes[i] = ConsumableController.instance.consumables[i].GetConsumableType();
        }
    }

    private void SaveGame()
    {
        isGameSaved = true;
        SaveSnake();
        SaveObstacles();
        SaveScore();
        SaveConsumables();

        OnSaveData?.Invoke();
    }

    private void ClearSavedGame() // clears all saved data
    {
        isGameSaved = false;

        Array.Clear(rockObstaclesAddresses, 0, rockObstaclesAddresses.Length);
        Array.Clear(snakeSegmentsAddresses, 0, snakeSegmentsAddresses.Length);
        Array.Clear(consumableListAddresses, 0, consumableListAddresses.Length);
        Array.Clear(consumableListTypes, 0, consumableListTypes.Length);
        consumableListAmount = 0;
        rockObstaclesAmount = 0;
        snakeSegmentsAmount = 0;
        lastMoveDirection = Directions.West;

        OnSaveData?.Invoke();
    }

    public void CalculateGenerateStartPoint()
    {
        generateLevelStartPoint = new Vector3(-((levelWidth - gridSize) / 2f), 0, -((levelHeight - gridSize) / 2f));        
    }
}
