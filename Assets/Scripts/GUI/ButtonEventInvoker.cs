using UnityEngine;
using System;

public class ButtonEventInvoker : MonoBehaviour
{
    public static event Action OnResumeButtonPressed;
    public static event Action OnRestartButtonPressed;
    public static event Action OnQuitButtonPressed;

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
}
