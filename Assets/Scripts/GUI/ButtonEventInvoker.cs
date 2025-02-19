using UnityEngine;
using System;

public class ButtonEventInvoker : MonoBehaviour
{
    public static event Action OnContinueButtonPressed;

    public void ContinueButtonPressed()
    {
        OnContinueButtonPressed?.Invoke();
    }        
}
