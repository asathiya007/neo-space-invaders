using UnityEngine;
using UnityEngine.SceneManagement;


// utility class and function to load scene
public class SceneLoader : MonoBehaviour {
    private const string GameSceneName = "NeoSpaceInvaders";
    private const string StartSceneName = "StartScreen";
    private const string GameOverSceneName = "GameOverScreen";
    private const string WinSceneName = "WinScreen";
    private const string LoadSceneName = "LoadScreen";

    // load game scene
    public static void LoadGameScene() {
        SceneManager.LoadScene(GameSceneName);
    }

    // load start scene
    public static void LoadStartScene() {
        SceneManager.LoadScene(StartSceneName);
    }

    // load loading scene
    public static void LoadLoadingScene() {
        SceneManager.LoadScene(LoadSceneName);
    }

    // load game over scene
    public static void LoadGameOverScene() {
        SceneManager.LoadScene(GameOverSceneName);
    }

    // load win scene
    public static void LoadWinScene() {
        SceneManager.LoadScene(WinSceneName);
    }
}
