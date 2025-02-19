using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;

    private void Awake()
    {
        ClosePauseMenu();
    }

    private void OnEnable()
    {
        GameStateController.OnPause += OpenPauseMenu;
        GameStateController.OnPlaying += ClosePauseMenu;
    }

    private void OnDisable()
    {
        GameStateController.OnPause -= OpenPauseMenu;
        GameStateController.OnPlaying -= ClosePauseMenu;
    }

    private void OpenPauseMenu()
    {
        pauseMenu.SetActive(true);        
    }

    private void ClosePauseMenu()
    {
        pauseMenu.SetActive(false);
    }
}
