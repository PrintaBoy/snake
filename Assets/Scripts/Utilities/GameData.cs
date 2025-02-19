using UnityEngine;

[System.Serializable]
public class GameData
{
    public static GameData gameData; // singleton
    public int levelWidth;
    public int levelHeight;
    public float gridSize;
    public Vector3 generateLevelStartPoint;
    
    public int currentScore;
    public int bestScore;

    public float gameSpeedMultiplier;
    public float gameTickLength;

    public int startSnakeLength;
    public float snakeSpeedMultiplier;
    public float snakeTickLength;

    private void Awake()
    {
        gameData = this;        
    }

    public void CalculateGenerateStartPoint()
    {
        generateLevelStartPoint = new Vector3(-((levelWidth - gridSize) / 2f), 0, -((levelHeight - gridSize) / 2f));        
    }
}
