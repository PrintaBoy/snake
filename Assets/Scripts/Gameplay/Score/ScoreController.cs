using UnityEngine;
using System;

public class ScoreController : MonoBehaviour
{
    public static int scoreCurrent {  get; private set; }
    public static int applesCollected { get; private set; }

    public static event Action OnScoreUpdated;

    private void OnEnable()
    {
        Apple.OnAppleConsumed += AppleConsumed;
    }

    private void OnDisable()
    {
        Apple.OnAppleConsumed -= AppleConsumed;
    }

    private void Awake()
    {
        scoreCurrent = 0;
        applesCollected = 0;
    }

    private void AppleConsumed(Apple apple)
    {
        ModifyScoreValue(apple.scoreValue);
    }

    public void ModifyScoreValue(int amount)
    {
        scoreCurrent += amount;
        applesCollected++;
        OnScoreUpdated?.Invoke();
    }
}
