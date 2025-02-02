[System.Serializable]
public class GameData
{
    public static GameData gameData;
    public int levelWidth;
    public int levelHeight;
    public int gridSize;
    
    public int currentScore;
    public int bestScore;

    public float snakeMovementSpeed;
    public int snakeLength;

    private void Awake()
    {
        gameData = this;
    }
}
