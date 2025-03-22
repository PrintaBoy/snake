using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PauseMenuGUIController : MonoBehaviour
{
    /// <summary>
    /// This class handles GUI of pause menu (appears when game state is pause) and game over menu (appears when game state is game over)
    /// </summary>
    
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject pauseMainMenu;
    [SerializeField] private GameObject gameOverSection;
    [SerializeField] private GameObject exitToMenuPrompt;
    [SerializeField] private GameObject quitGamePrompt;
    [SerializeField] private Button resumeButton;

    [Header("Game over score section")]
    [SerializeField] private TMP_Text appleAmountText;
    [SerializeField] private TMP_Text appleScoreText;
    [SerializeField] private TMP_Text pumpkinAmountText;
    [SerializeField] private TMP_Text pumpkinScoreText;
    [SerializeField] private TMP_Text mushroomAmountText;
    [SerializeField] private TMP_Text mushroomScoreText;
    [SerializeField] private TMP_Text acornAmountText;
    [SerializeField] private TMP_Text acornScoreText;
    [SerializeField] private TMP_Text grapeAmountText;
    [SerializeField] private TMP_Text grapeScoreText;
    [SerializeField] private TMP_Text totalScoreText;       

    private void OnEnable()
    {
        GameStateController.OnStart += ClosePauseMenu;
        GameStateController.OnPause += OpenPauseMenu;
        GameStateController.OnPause += OpenPauseMainMenu;
        GameStateController.OnPlaying += ClosePauseMainMenu;
        GameStateController.OnPlaying += ClosePauseMenu;
        GameStateController.OnGameOver += ShowGameOverSection;
        ButtonEventInvoker.OnExitToMenuButtonPressed += ShowExitToMenuPrompt;
        ButtonEventInvoker.OnExitToMenuNoButtonPressed += HideExitToMenuPrompt;
        ButtonEventInvoker.OnQuitGameButtonPressed += ShowQuitGamePrompt;
        ButtonEventInvoker.OnQuitGameBackButtonPressed += HideQuitGamePrompt;
    }

    private void OnDisable()
    {
        GameStateController.OnStart -= ClosePauseMenu;
        GameStateController.OnPause -= OpenPauseMenu;
        GameStateController.OnPause -= OpenPauseMainMenu;
        GameStateController.OnPlaying -= ClosePauseMainMenu;
        GameStateController.OnPlaying -= ClosePauseMenu;
        GameStateController.OnGameOver -= ShowGameOverSection;
        ButtonEventInvoker.OnExitToMenuButtonPressed -= ShowExitToMenuPrompt;
        ButtonEventInvoker.OnExitToMenuNoButtonPressed -= HideExitToMenuPrompt;
        ButtonEventInvoker.OnQuitGameButtonPressed -= ShowQuitGamePrompt;
        ButtonEventInvoker.OnQuitGameBackButtonPressed -= HideQuitGamePrompt;
    }

    private void DisplayGameOverScoreValues()
    {
        appleAmountText.text = "x " + ScoreController.applesConsumed.ToString();
        appleScoreText.text = (ScoreController.applesConsumed * GameData.gameData.appleScoreValue).ToString();

        pumpkinAmountText.text = "x " + ScoreController.pumpkinsConsumed.ToString();
        pumpkinScoreText.text = (ScoreController.pumpkinsConsumed * GameData.gameData.pumpkinScoreValue).ToString();

        mushroomAmountText.text = "x " + ScoreController.mushroomsConsumed.ToString();
        mushroomScoreText.text = (ScoreController.mushroomsConsumed * GameData.gameData.mushroomScoreValue).ToString();

        acornAmountText.text = "x " + ScoreController.acornsConsumed.ToString();
        acornScoreText.text = (ScoreController.acornsConsumed * GameData.gameData.acornScoreValue).ToString();

        grapeAmountText.text = "x " + ScoreController.grapesConsumed.ToString();
        grapeScoreText.text = (ScoreController.grapesConsumed * GameData.gameData.grapeScoreValue).ToString();

        totalScoreText.text = ScoreController.scoreCurrent.ToString();
    }

    private void ShowGameOverSection()
    {
        OpenPauseMenu();
        gameOverSection.SetActive(true);
        DisplayGameOverScoreValues();
        OpenPauseMainMenu(); // TODO open pause main menu only after score is counted and shown to player
    }

    private void HideGameOverSection()
    {
        gameOverSection.SetActive(false);
        ClosePauseMenu();
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
        pauseMenu.SetActive(true);        
    }

    private void ClosePauseMenu()
    {
        pauseMenu.SetActive(false);
    }
}
