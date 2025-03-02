using UnityEngine;
using System;

public class ScoreController : MonoBehaviour
{
    public static int scoreCurrent {  get; private set; }
    public static int applesConsumed { get; private set; }
    public static int pumpkinsConsumed { get; private set; }
    public static int mushroomConsumed { get; private set; }

    public static event Action OnScoreUpdated;

    private void OnEnable()
    {
        Apple.OnAppleConsumed += AppleConsumed;
        Pumpkin.OnPumpkinConsumed += PumpkinConsumed;
        Mushroom.OnMushroomConsumed += MushroomConsumed;
    }

    private void OnDisable()
    {
        Apple.OnAppleConsumed -= AppleConsumed;
        Pumpkin.OnPumpkinConsumed -= PumpkinConsumed;
        Mushroom.OnMushroomConsumed -= MushroomConsumed;
    }

    private void Awake()
    {
        scoreCurrent = 0;
        applesConsumed = 0;
        pumpkinsConsumed = 0;
        mushroomConsumed = 0;
        OnScoreUpdated?.Invoke();
    }

    private void PumpkinConsumed(Pumpkin pumpkin)
    {
        pumpkinsConsumed++;
        ModifyScoreValue(pumpkin.scoreValue);        
    }

    private void AppleConsumed(Apple apple)
    {
        applesConsumed++;
        ModifyScoreValue(apple.scoreValue);        
    }

    private void MushroomConsumed(Mushroom mushroom)
    {
        mushroomConsumed++;
        ModifyScoreValue(mushroom.scoreValue);  
    }

    public void ModifyScoreValue(int amount)
    {
        scoreCurrent += amount;
        OnScoreUpdated?.Invoke();
    }
}
