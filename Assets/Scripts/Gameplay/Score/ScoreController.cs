using UnityEngine;
using System;

public class ScoreController : MonoBehaviour
{
    public static int scoreCurrent {  get; private set; }
    public static int applesConsumed { get; private set; }
    public static int pumpkinsConsumed { get; private set; }
    public static int mushroomsConsumed { get; private set; }
    public static int acornsConsumed { get; private set; }
    public static int grapesConsumed { get; private set; }
    public static int scoreHighest { get; private set; }

    public static event Action OnScoreUpdated;
    public static event Action OnNewHighScore;

    private void OnEnable()
    {
        Apple.OnAppleConsumed += AppleConsumed;
        Pumpkin.OnPumpkinConsumed += PumpkinConsumed;
        Mushroom.OnMushroomConsumed += MushroomConsumed;
        Acorn.OnAcornConsumed += AcornConsumed;
        Grape.OnGrapeConsumed += GrapeConsumed;
    }

    private void OnDisable()
    {
        Apple.OnAppleConsumed -= AppleConsumed;
        Pumpkin.OnPumpkinConsumed -= PumpkinConsumed;
        Mushroom.OnMushroomConsumed -= MushroomConsumed;
        Acorn.OnAcornConsumed -= AcornConsumed;
        Grape.OnGrapeConsumed -= GrapeConsumed;
    }

    private void Awake()
    {
        scoreCurrent = 0;
        applesConsumed = 0;
        pumpkinsConsumed = 0;
        mushroomsConsumed = 0;
        acornsConsumed = 0;
        grapesConsumed = 0;
        scoreHighest = GameData.gameData.highestScore;
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
        mushroomsConsumed++;
        ModifyScoreValue(mushroom.scoreValue);  
    }

    private void AcornConsumed(Acorn acorn)
    {
        acornsConsumed++;
        ModifyScoreValue(acorn.scoreValue);
    }

    private void GrapeConsumed(Grape grape)
    {
        grapesConsumed++;
        ModifyScoreValue(grape.scoreValue);
    }

    public void ModifyScoreValue(int amount)
    {
        scoreCurrent += amount;

        if (scoreCurrent > scoreHighest)
        {            
            scoreHighest = scoreCurrent;
            OnNewHighScore?.Invoke();
        }       

        OnScoreUpdated?.Invoke();
    }
}
