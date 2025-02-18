using UnityEngine;
using System;

public class GameStateController : MonoBehaviour
{
    [HideInInspector] public static GameStates gameState {  get; private set; }
    public static event Action OnGameOver; // event is called when game is over 
    public static event Action OnPlaying;

    private void Awake()
    {
        ChangeState(GameStates.Start);
    }

    private void OnEnable()
    {
        SnakeSegment.OnObstacleCollision += ObstacleCollision;
        SnakeController.OnValidMove += MoveStarted;
    }

    private void OnDisable()
    {
        SnakeSegment.OnObstacleCollision -= ObstacleCollision;
        SnakeController.OnValidMove -= MoveStarted;
    }

    private void ObstacleCollision()
    {
        if (gameState == GameStates.Playing)
        {
            ChangeState(GameStates.GameOver);
        }       
    }

    private void MoveStarted() // when player hits correct movement input, the game will start playing
    {
        if (gameState == GameStates.Start) // prevents the game starting from other states than Start
        {
            ChangeState(GameStates.Playing);
        }        
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
                PlayingState();
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

    private void PlayingState()
    {
        OnPlaying?.Invoke();    
    }
}
