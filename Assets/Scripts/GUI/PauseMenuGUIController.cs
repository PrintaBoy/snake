using UnityEngine;
using UnityEngine.UI;

public class PauseMenuGUIController : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
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
    }

    private void OnDisable()
    {
        GameStateController.OnPause -= OpenPauseMenu;
        GameStateController.OnPlaying -= ClosePauseMenu;
        GameStateController.OnGameOver -= OpenPauseMenu;
    }

    private void OpenPauseMenu()
    {
        resumeButton.interactable = GameStateController.gameState == GameStates.GameOver ? false : true; // disables or enables resume button based on game state        
        pauseMenu.SetActive(true);        
    }

    private void ClosePauseMenu()
    {
        pauseMenu.SetActive(false);
    }
}
