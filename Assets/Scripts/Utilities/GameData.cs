using UnityEngine;

[System.Serializable]
public class GameData
{
    public static GameData gameData; // singleton
    public int levelWidth;
    public int levelHeight;
    public int gridSize;
    public Vector3 generateLevelStartPoint;
    
    public int currentScore;
    public int bestScore;

    public float gameSpeedMultiplier;
    public int startSnakeLength;
    public float tickLength;

    private void Awake()
    {
        gameData = this;        
    }

    public void CalculateGenerateStartPoint()
    {
        generateLevelStartPoint = new Vector3(-((levelWidth - gridSize) / 2f), 0, -((levelHeight - gridSize) / 2f));        
    }
}
