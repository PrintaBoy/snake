using UnityEngine;

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
    public int bestScore;

    // data for Game Tick
    public float gameSpeedMultiplier;
    public float gameTickLength;

    // data for snake
    public int startSnakeLength;
    public float snakeSpeedMultiplier;
    public float snakeSpeedMaxMultiplier;
    public float snakeTickLength;

    // data for Apple consumable
    public int appleAddSnakeSegmentAmount;
    public float appleSnakeSpeedChange;
    public int appleScoreValue;

    // data for Obstacle
    public int obstacleSpawnRate; // how long before next obstacle is spawned, measured in Game Ticks
    public int obstacleSpawnDuration; // how long obstacle will stay on grid before it dissapears, measured in Game Ticks
    public int obstacleMaxSpawnCount; // how many obstacles can be on grid at the same time

    private void Awake()
    {
        gameData = this;        
    }

    public void CalculateGenerateStartPoint()
    {
        generateLevelStartPoint = new Vector3(-((levelWidth - gridSize) / 2f), 0, -((levelHeight - gridSize) / 2f));        
    }
}
