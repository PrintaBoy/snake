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

    // data for primary game tick
    public float gameSpeedMultiplier;
    public float gameTickLength;

    // data for snake
    public int startSnakeLength;
    public float snakeSpeedMultiplier;
    public float snakeTickLength;

    // data for Apple consumable
    public int appleAddSnakeSegmentAmount;
    public float appleSnakeSpeedChange;

    private void Awake()
    {
        gameData = this;        
    }

    public void CalculateGenerateStartPoint()
    {
        generateLevelStartPoint = new Vector3(-((levelWidth - gridSize) / 2f), 0, -((levelHeight - gridSize) / 2f));        
    }
}
