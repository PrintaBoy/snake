using UnityEngine;
using System;

public class GameStateController : MonoBehaviour
{
    public static bool isGameOver = false;
    [HideInInspector] public static GameStates gameState {  get; private set; }
    public static event Action OnSceneLoaded;
    public static event Action OnStart; // event is called when game started, fresh from loading
    public static event Action OnGameOver; // event is called when game is over 
    public static event Action OnPlaying;
    public static event Action OnPause;
    public static event Action OnSceneRestart;

    private void Awake()
    {        
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

    public void ChangePauseState() // can be called by command or by button interaction, opens or closes pause menu
    {
        if (gameState == GameStates.PauseMenu && !isGameOver)
        {
            ChangeState(GameStates.Playing);
            return;
        }

        if (gameState == GameStates.Playing)
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
