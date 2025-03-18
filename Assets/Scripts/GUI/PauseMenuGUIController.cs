using UnityEngine;
using UnityEngine.UI;

public class PauseMenuGUIController : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject pauseMainMenu;
    [SerializeField] private GameObject exitToMenuPrompt;
    [SerializeField] private GameObject quitGamePrompt;
    [SerializeField] private Button resumeButton;

    private void Awake()
    {
        ClosePauseMenu();
    }

    private void OnEnable()
    {
        GameStateController.OnPause += OpenPauseMenu;
        GameStateController.OnPlaying += ClosePauseMenu;
        GameStateController.OnGameOver += OpenPauseMenu;
        ButtonEventInvoker.OnExitToMenuButtonPressed += ShowExitToMenuPrompt;
        ButtonEventInvoker.OnExitToMenuNoButtonPressed += HideExitToMenuPrompt;
        ButtonEventInvoker.OnQuitGameButtonPressed += ShowQuitGamePrompt;
        ButtonEventInvoker.OnQuitGameBackButtonPressed += HideQuitGamePrompt;
    }

    private void OnDisable()
    {
        GameStateController.OnPause -= OpenPauseMenu;
        GameStateController.OnPlaying -= ClosePauseMenu;
        GameStateController.OnGameOver -= OpenPauseMenu;
        ButtonEventInvoker.OnExitToMenuButtonPressed -= ShowExitToMenuPrompt;
        ButtonEventInvoker.OnExitToMenuNoButtonPressed -= HideExitToMenuPrompt;
        ButtonEventInvoker.OnQuitGameButtonPressed -= ShowQuitGamePrompt;
        ButtonEventInvoker.OnQuitGameBackButtonPressed -= HideQuitGamePrompt;
    }

    private void ShowQuitGamePrompt()
    {
        ClosePauseMainMenu();
        quitGamePrompt.SetActive(true);
    }

    private void HideQuitGamePrompt()
    {
        OpenPauseMainMenu();
        quitGamePrompt.SetActive(false);
    }

    private void ShowExitToMenuPrompt()
    {
        ClosePauseMainMenu();
        exitToMenuPrompt.SetActive(true);
    }

    private void HideExitToMenuPrompt()
    {
        OpenPauseMainMenu();
        exitToMenuPrompt.SetActive(false);
    }

    private void OpenPauseMainMenu() // opens only main menu (where the main buttons are)
    {
        pauseMainMenu.SetActive(true);
    }

    private void ClosePauseMainMenu() // close only main menu (where the main buttons are)
    {
        pauseMainMenu.SetActive(false);
    }

    private void OpenPauseMenu() // opens whole pause menu (parent) with background
    {
        resumeButton.interactable = GameStateController.gameState == GameStates.GameOver ? false : true; // disables or enables resume button based on game state        
        HideExitToMenuPrompt();
        HideQuitGamePrompt();
        pauseMenu.SetActive(true);        
    }

    private void ClosePauseMenu()
    {
        pauseMenu.SetActive(false);
    }
}
