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
    
    // data for score
    public int currentScore;
    public int highestScore;

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
    public int snakeSegmentsAmount;
    public Vector2Int snakeHeadAddress;
    public Vector2Int[] snakeSegmentsAddresses;
    public int rockObstaclesAmount;
    public Vector2Int[] rockObstaclesAddresses;

    public static event Action OnSaveData;

    public void OnAwake() // called from other function because this class is non-MonoBehavior
    {
        gameData = this;
        ScoreController.OnNewHighScore += SaveNewHighScore;
        ButtonEventInvoker.OnQuitButtonPressed += SaveGame;
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

        snakeHeadAddress = SnakeController.instance.snakeSegments[0].GetParentGridAddress(); // save head

        for (int i = 1; i < snakeSegmentsAmount; i++)
        {
            snakeSegmentsAddresses[i] = SnakeController.instance.snakeSegments[i].GetParentGridAddress(); // save tail
        }

        // TODO - also save direction of snake movement
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

    private void SaveGame()
    {
        isGameSaved = true;
        SaveSnake();
        SaveObstacles();

        OnSaveData?.Invoke();
    }

    private void ClearSavedGame() // clears all saved data
    {
        isGameSaved = false;

        Array.Clear(rockObstaclesAddresses, 0, rockObstaclesAddresses.Length);
        Array.Clear(snakeSegmentsAddresses, 0, snakeSegmentsAddresses.Length);
        rockObstaclesAmount = 0;
        snakeSegmentsAmount = 0;

        OnSaveData?.Invoke();
    }

    public void CalculateGenerateStartPoint()
    {
        generateLevelStartPoint = new Vector3(-((levelWidth - gridSize) / 2f), 0, -((levelHeight - gridSize) / 2f));        
    }
}
