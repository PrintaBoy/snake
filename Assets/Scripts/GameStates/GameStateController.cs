using UnityEngine;
using System;

public class GameStateController : MonoBehaviour
{
    /// <summary>
    /// This class handles game states (if the game is paused, started, game over...)
    /// It keeps track of current state and also invoke events when state is changed
    /// </summary>
    
    public static bool isGameOver = false;
    public static bool isSnakeMoving = false; // determines if player started the level by moving snake
    [HideInInspector] public static GameStates gameState {  get; private set; }
    public static event Action OnSceneLoaded;
    public static event Action OnStart; // event is called when game started, fresh from loading
    public static event Action OnGameOver; // event is called when game is over 
    public static event Action OnPlaying; // event is called when player started playing the level (snake is moving)
    public static event Action OnPause;
    public static event Action OnSceneRestart;

    private void Awake()
    {
        isSnakeMoving = false; 
        isGameOver = false;
        ChangeState(GameStates.Start);
    }

    private void Start()
    {
        OnSceneLoaded?.Invoke();
    }

    private void OnEnable()
    {
        SnakeSegment.OnSnakeSegmentCollision += ObstacleCollision;
        Rock.OnObstacleCollision += ObstacleCollision;
        SnakeController.OnValidMove += MoveStarted;
        ButtonEventInvoker.OnRestartButtonPressed += RestartButtonPressed;
    }

    private void OnDisable()
    {
        SnakeSegment.OnSnakeSegmentCollision -= ObstacleCollision;
        Rock.OnObstacleCollision -= ObstacleCollision;
        SnakeController.OnValidMove -= MoveStarted;
        ButtonEventInvoker.OnRestartButtonPressed -= RestartButtonPressed;
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
            isSnakeMoving = true;
        }        
    }

    private void ChangeState(GameStates switchToState)
    {
        switch (switchToState)
        {
            case GameStates.PauseMenu:
                gameState = GameStates.PauseMenu;
                PauseState();
                break;
            case GameStates.Start:
                gameState = GameStates.Start;
                StartState();
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

    private void PauseState()
    {
        OnPause?.Invoke();
    }

    private void StartState()
    {
        OnStart?.Invoke();
        isGameOver = false;
    }

    private void GameOverState()
    {
        OnGameOver?.Invoke();
        isGameOver = true;
    }

    private void PlayingState()
    {
        OnPlaying?.Invoke();    
    }
    
    public void ChangePauseState()
    {
        /// <summary>
        /// can be called by command or by button interaction, serves to pause the game
        /// </summary>

        if (gameState == GameStates.PauseMenu && !isGameOver)
        {
            if (isSnakeMoving)
            {
                ChangeState(GameStates.Playing);
            }
            else
            {
                ChangeState(GameStates.Start);
            }
            return;
        }

        if (gameState == GameStates.Playing || gameState == GameStates.Start)
        {
            ChangeState(GameStates.PauseMenu);
            return;
        }        
    }

    private void RestartLevel()
    {
        isGameOver = false;
        OnSceneRestart?.Invoke();
    }

    private void RestartButtonPressed()
    {
        RestartLevel();
    }
}
