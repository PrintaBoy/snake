using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class SceneController : MonoBehaviour
{
    /// <summary>
    /// Scene indexes
    /// 0 = Bootstrapper
    /// 1 = MainMenu
    /// 2 = SnakeLevel
    /// </summary>
    private int loadSceneIndex = 1; // this variable is changed based on which scene to load, default is MainMenu
    public static bool isNewGame; // if true, player started new game. If false, player continues previous saved game

    private void OnEnable()
    {
        GameStateController.OnSceneLoaded += SceneLoaded;
        GameStateController.OnSceneRestart += SceneRestart;
        ButtonEventInvoker.OnQuitGameQuitButtonPressed += QuitGame;
        ButtonEventInvoker.OnExitToMenuYesButtonPressed += ExitToMenu;
        ButtonEventInvoker.OnNewGameButtonPressed += NewGame;
        ButtonEventInvoker.OnContinueButtonPressed += Continue;
    }

    private void OnDisable()
    {
        GameStateController.OnSceneLoaded -= SceneLoaded;
        GameStateController.OnSceneRestart -= SceneRestart;
        ButtonEventInvoker.OnQuitGameQuitButtonPressed -= QuitGame;
        ButtonEventInvoker.OnExitToMenuYesButtonPressed -= ExitToMenu;
        ButtonEventInvoker.OnNewGameButtonPressed -= NewGame;
        ButtonEventInvoker.OnContinueButtonPressed -= Continue;
    }

    private void Awake()
    {
        LoadScene(loadSceneIndex);
    }

    public void ChangeLoadSceneIndex(int sceneIndex)
    {
        loadSceneIndex = sceneIndex;
    }

    private void UnloadScene(int sceneIndexNumber)
    {
        SceneManager.UnloadSceneAsync(sceneIndexNumber);
    }

    private void LoadScene(int sceneIndexNumber)
    {
        SceneManager.LoadScene(sceneIndexNumber, LoadSceneMode.Additive);
    }

    private void RestartActiveScene()
    {
        Scene activeScene = SceneManager.GetActiveScene();        
        ChangeLoadSceneIndex(activeScene.buildIndex);
        UnloadScene(loadSceneIndex);
        LoadScene(loadSceneIndex);
    }

    private void SetActiveScene() // is set up automatically when scene is switched
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(loadSceneIndex));
    }

    private void NewGame() // starts SnakeLevel scene as new game
    {
        isNewGame = true;
        UnloadScene(loadSceneIndex);
        ChangeLoadSceneIndex(2);
        LoadScene(loadSceneIndex);        
    }

    private void Continue() // starts SnakeLevel scene and continues game from last save
    {
        isNewGame = false;
        UnloadScene(loadSceneIndex);
        ChangeLoadSceneIndex(2);
        LoadScene(loadSceneIndex);
    }

    private void QuitGame()
    {
        # if UNITY_EDITOR
        EditorApplication.isPlaying = false;
        #endif
        Application.Quit();        
    }

    private void ExitToMenu()
    {
        UnloadScene(loadSceneIndex);
        ChangeLoadSceneIndex(1);
        LoadScene(loadSceneIndex);
    }

    private void SceneLoaded() // method call when OnSceneLoaded event is catched
    {
        SetActiveScene();
    }

    private void SceneRestart() // method call when OnSceneRestart event is catched
    {
        isNewGame = true;
        RestartActiveScene();        
    }
}
