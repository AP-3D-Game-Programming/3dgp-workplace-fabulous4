using UnityEngine;

public class ButtonOnClickSceneLoader : MonoBehaviour
{
    public void LoadSceneByName(string name)
    {
        ScreenManager.LoadScene(name);
    }

    public void LoadSceneByEnum(Scenes scene)
    {
        ScreenManager.LoadScene(scene);
    }

    public void QuitGame()
    {
        ScreenManager.QuitGame();
    }
}
