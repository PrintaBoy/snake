using UnityEngine;
using System;
using UnityEngine.SocialPlatforms.Impl;

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
        Consumable.OnConsumableConsumed += ConsumableConsumed;
    }

    private void OnDisable()
    {
        Consumable.OnConsumableConsumed -= ConsumableConsumed;
    }

    private void Awake()
    {        
        applesConsumed = 0;
        pumpkinsConsumed = 0;
        mushroomsConsumed = 0;
        acornsConsumed = 0;
        grapesConsumed = 0;
        scoreHighest = GameData.gameData.highestScore;
        scoreCurrent = SceneController.isNewGame ? 0 : GameData.gameData.currentScore; // either sets score to zero or loads score from JSON       

        OnScoreUpdated?.Invoke();
    }

    private void ConsumableConsumed(ConsumableTypes consumableType)
    {
        switch (consumableType)
        {
            case ConsumableTypes.Apple:
                applesConsumed++;
                ModifyScoreValue(GameData.gameData.appleScoreValue);
                break;
            case ConsumableTypes.Acorn:
                acornsConsumed++;
                ModifyScoreValue(GameData.gameData.acornScoreValue);
                break;
            case ConsumableTypes.Grape: 
                grapesConsumed++;
                ModifyScoreValue(GameData.gameData.grapeScoreValue);
                break;
            case ConsumableTypes.Mushroom:
                mushroomsConsumed++;
                ModifyScoreValue(GameData.gameData.mushroomScoreValue);
                break;
            case ConsumableTypes.Pumpkin:
                pumpkinsConsumed++;
                ModifyScoreValue(GameData.gameData.pumpkinScoreValue);
                break;
        }
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
