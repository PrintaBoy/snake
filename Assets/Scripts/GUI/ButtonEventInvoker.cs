using UnityEngine;
using System;

public class ButtonEventInvoker : MonoBehaviour
{
    public static event Action OnContinueButtonPressed;
    public static event Action OnRestartButtonPressed;
    public static event Action OnQuitButtonPressed;

    public void ContinueButtonPressed()
    {
        OnContinueButtonPressed?.Invoke();
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
