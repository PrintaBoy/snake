using UnityEngine;
using System;

public class ButtonEventInvoker : MonoBehaviour
{
    public static event Action OnResumeButtonPressed; // resumes current game from pause menu
    public static event Action OnRestartButtonPressed;
    public static event Action OnQuitGameButtonPressed;
    public static event Action OnQuitGameQuitButtonPressed;
    public static event Action OnQuitGameBackButtonPressed;
    public static event Action OnExitToMenuButtonPressed;
    public static event Action OnExitToMenuYesButtonPressed;
    public static event Action OnExitToMenuNoButtonPressed;
    public static event Action OnContinueButtonPressed; // continues last saved game from main menu
    public static event Action OnNewGameButtonPressed;
    
    public void ExitToMenuYesButtonPressed()
    {
        OnExitToMenuYesButtonPressed?.Invoke();
    }

    public void ExitToMenuNoButtonPressed()
    {
        OnExitToMenuNoButtonPressed?.Invoke();
    }

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
        OnQuitGameButtonPressed?.Invoke();
    }

    public void QuitGameQuitButtonPressed()
    {
        OnQuitGameQuitButtonPressed?.Invoke();
    }

    public void QuitGameBackButtonPressed()
    {
        OnQuitGameBackButtonPressed?.Invoke();
    }

    public void ExitToMenuButtonPressed()
    {
        OnExitToMenuButtonPressed?.Invoke();
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
