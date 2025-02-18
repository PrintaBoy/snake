using UnityEngine;
using System;

public class TickController : MonoBehaviour
{
    private bool tickTimerRunning = false;
    private float tickTimer = 0f; // current time of tick
    private float tickLength = 0.3f; // how long it takes from end of previous tick to next tick
    private float gameSpeedMultiplier = 1f; // this value multiplies Time.deltaTime based on player actions
    public static event Action OnTick;

    private void OnEnable()
    {
        SnakeController.OnValidMove += ValidMove;
        GameStateController.OnPlaying += GamePlaying;
        GameStateController.OnGameOver += GameOver;
    }

    private void OnDisable()
    {
        SnakeController.OnValidMove -= ValidMove;
        GameStateController.OnPlaying -= GamePlaying;
        GameStateController.OnGameOver -= GameOver;
    }

    private void Start()
    {        
        gameSpeedMultiplier = GameData.gameData.gameSpeedMultiplier;
        tickLength = GameData.gameData.tickLength;
    }

    private void Update()
    {
        if (!tickTimerRunning)
        {
            return;
        }

        tickTimer += Time.deltaTime * gameSpeedMultiplier;
        if (tickTimer >= tickLength)
        {
            OnTick?.Invoke();
            ResetTickTimer();
        }
    }

    private void ToggleTickTimer(bool toggle) // switches tick timer on and off
    {
        tickTimerRunning = toggle;
    }

    private void ResetTickTimer()
    {
        tickTimer = 0f;
    }

    private void ValidMove() // if snake makes valid move
    {
        ResetTickTimer();
    }

    private void GamePlaying()
    {
        ToggleTickTimer(true);
    }

    private void GameOver()
    {
        ToggleTickTimer(false); 
    }
}
