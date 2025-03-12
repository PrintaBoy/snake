using UnityEngine;
using UnityEngine.UI;

public class MainMenuGUIController : MonoBehaviour
{
    [SerializeField] private Button continueButton;

    private void Awake()
    {
        ToggleContinueButton();        
    }

    private void ToggleContinueButton() // enables or disables continue button based on JSON - if there is a game saved from which the player can continue to play
    {
        continueButton.interactable = GameData.gameData.isGameSaved ? true : false; // ternary operator, hell yeah        
    }
}
