using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class SceneController : MonoBehaviour
{
    private int loadSceneIndex = 1; // this variable is changed based on which scene to load, default is SnakeLevel for now (change later for MainMenu)

    private void OnEnable()
    {
        GameStateController.OnSceneLoaded += SceneLoaded;
        GameStateController.OnSceneRestart += SceneRestart;
        ButtonEventInvoker.OnQuitButtonPressed += QuitGame;
    }

    private void OnDisable()
    {
        GameStateController.OnSceneLoaded -= SceneLoaded;
        GameStateController.OnSceneRestart -= SceneRestart;
        ButtonEventInvoker.OnQuitButtonPressed -= QuitGame;
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

    private void SetActiveScene()
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(loadSceneIndex));
    }

    private void QuitGame()
    {
        # if UNITY_EDITOR
        EditorApplication.isPlaying = false;
        #endif
        Application.Quit();        
    }

    private void SceneLoaded() // method call when OnSceneLoaded event is catched
    {
        SetActiveScene();
    }

    private void SceneRestart() // method call when OnSceneRestart event is catched
    {
        RestartActiveScene();
    }
}
