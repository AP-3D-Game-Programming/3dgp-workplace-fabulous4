using UnityEngine.SceneManagement;

public enum Scenes
{
    StartScreen,
    TutorialScene,
    EndTutorialScreen,
    GameScreen,
    EndLevelScreen
}

public static class ScreenManager
{
    public static void LoadScene(Scenes scene)
    {
        SceneManager.LoadScene(scene.ToString());
    }

    public static void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public static void QuitGame()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #endif
    }
}
