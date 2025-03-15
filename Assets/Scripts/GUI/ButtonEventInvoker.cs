using UnityEngine;
using System;

public class ButtonEventInvoker : MonoBehaviour
{
    public static event Action OnResumeButtonPressed; // resumes current game from pause menu
    public static event Action OnRestartButtonPressed;
    public static event Action OnQuitButtonPressed;
    public static event Action OnContinueButtonPressed; // continues last saved game from main menu
    public static event Action OnNewGameButtonPressed; 

    public void ResumeButtonPressed()
    {
        OnResumeButtonPressed?.Invoke();
    }

    public void RestartButtonPressed()
    {
        OnRestartButtonPressed?.Invoke();
    }

    public void QuitButtonPressed()
    {
        OnQuitButtonPressed?.Invoke();
    }

    public void NewGameButtonPressed()
    {
        OnNewGameButtonPressed?.Invoke();
    }

    public void ContinueButtonPressed()
    {
        OnContinueButtonPressed?.Invoke();
    }
}
