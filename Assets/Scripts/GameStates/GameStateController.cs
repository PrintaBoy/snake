using UnityEngine;
using System;

public enum GameStates
{
    PauseMenu, // player is in pause menu
    Start, // waiting for player to start playing level
    Playing, // player playing level
    GameOver // player lost level
}

public class GameStateController : MonoBehaviour
{
    [HideInInspector] public static GameStates gameState {  get; private set; }
    public static event Action OnGameOver; // event is called when game is over 

    private void OnEnable()
    {
        SnakeSegment.OnObstacleCollision += ObstacleCollision;
    }

    private void OnDisable()
    {
        SnakeSegment.OnObstacleCollision -= ObstacleCollision;
    }

    private void ObstacleCollision()
    {
        ChangeState(GameStates.GameOver);
    }

    private void ChangeState(GameStates switchToState)
    {
        switch (switchToState)
        {
            case GameStates.PauseMenu:
                gameState = GameStates.PauseMenu;
                break;
            case GameStates.Start:
                gameState = GameStates.Start;
                break;
            case GameStates.Playing:
                gameState = GameStates.Playing;
                break;
            case GameStates.GameOver:
                gameState = GameStates.GameOver;
                GameOverState();
                break;
        }
    }

    private void GameOverState()
    {
        OnGameOver?.Invoke();
    }
}
