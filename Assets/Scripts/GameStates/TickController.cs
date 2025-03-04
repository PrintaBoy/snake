using UnityEngine;
using System;

public class TickController : MonoBehaviour
{
    /// <summary>
    /// This class controls all ticks in the game. 
    /// Main Tick system (Game) is for controlling spawn rate of consumables, obstacles, etc.
    /// Secondary Tick system (Snake) is for controlling speed of automatic movement for snake
    /// Tick systems need to be separated for game design reasons (to properly implement slow mo, speeding up/slowing down snake, etc.)
    /// </summary>


    private bool tickTimerRunning = false;
    private bool isGameTickSpeedModified = false;

    private int gameTickModifiedDuration = 0; // how long should the game tick speed modification last, in ticks
    private int gameTickModifiedTickCounter = 0;

    private float gameTickTimer = 0f; // current time of tick
    private float gameTickLength = 0f; // how long it takes from end of previous tick to next tick
    private float gameTickSpeedMultiplier = 1f; // this value multiplies Time.deltaTime based on player actions    

    private float snakeTickTimer = 0f; // current time of snake tick
    private float snakeTickLength = 0f; // how long it takes from end of previous tick to next tick for snake    
   
    public static event Action OnGameTick;
    public static event Action OnSnakeTick;

    private void OnEnable()
    {
        SnakeController.OnValidMove += ValidMove;
        GameStateController.OnPlaying += EnableTickTimer;
        GameStateController.OnGameOver += DisableTickTimer;
        GameStateController.OnPause += DisableTickTimer;
        Grape.OnGrapeConsumed += GrapeConsumed;
    }

    private void OnDisable()
    {
        SnakeController.OnValidMove -= ValidMove;
        GameStateController.OnPlaying -= EnableTickTimer;
        GameStateController.OnGameOver -= DisableTickTimer;
        GameStateController.OnPause -= DisableTickTimer;
        Grape.OnGrapeConsumed -= GrapeConsumed;
    }

    private void Start()
    {        
        gameTickSpeedMultiplier = GameData.gameData.gameSpeedMultiplier;
        gameTickLength = GameData.gameData.gameTickLength;

        snakeTickLength = GameData.gameData.snakeTickLength;
    }

    private void Update()
    {
        if (!tickTimerRunning)
        {
            return;
        }

        gameTickTimer += Time.deltaTime * gameTickSpeedMultiplier;
        if (gameTickTimer >= gameTickLength)
        {
            OnGameTick?.Invoke();
            ResetGameTickTimer();

            if (isGameTickSpeedModified)
            {
                ModifyGameSpeedCounter();
            }
        }

        snakeTickTimer += Time.deltaTime * SnakeController.snakeSpeedMultiplier;
        if (snakeTickTimer >= snakeTickLength)
        {
            OnSnakeTick?.Invoke();
            ResetSnakeTickTimer();
        }
    }

    private void ToggleTickTimer(bool toggle) // switches tick timer on and off
    {
        tickTimerRunning = toggle;
    }

    private void ModifyGameTickSpeedMultiplier(float modifyAmount, int gameTickSpeedChangeDuration)
    {
        gameTickSpeedMultiplier += modifyAmount;
        gameTickModifiedDuration = gameTickSpeedChangeDuration;
        isGameTickSpeedModified = true;
        Debug.Log(gameTickSpeedMultiplier);
    }
    
    private void ModifyGameSpeedCounter() // this method counts how long the game speed should remain modified. Resets the game speed back once it expires
    {
        gameTickModifiedTickCounter++;
        if (gameTickModifiedTickCounter >= gameTickModifiedDuration)
        {
            gameTickSpeedMultiplier = GameData.gameData.gameSpeedMultiplier;
            isGameTickSpeedModified = false;
            gameTickModifiedTickCounter = 0;
            Debug.Log(gameTickSpeedMultiplier);
        }
    }

    private void GrapeConsumed(Grape grape)
    {
        ModifyGameTickSpeedMultiplier(-grape.gameSpeedChange, grape.gameSpeedChangeDuration);
    }

    private void ResetGameTickTimer()
    {
        gameTickTimer = 0f;
    }

    private void ResetSnakeTickTimer()
    {
        snakeTickTimer = 0f;
    }

    private void ValidMove() // if snake makes valid move
    {
        ResetSnakeTickTimer();
    }

    private void DisableTickTimer()
    {
        ToggleTickTimer(false);
    }

    private void EnableTickTimer()
    {
        ToggleTickTimer(true);
    }
}
