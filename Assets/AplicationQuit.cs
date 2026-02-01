using UnityEngine;

public class ApplicationQuit : MonoBehaviour
{
    private void OnMouseDown()
    {
        QuitGame();
    }

    private void QuitGame()
    {
        Debug.Log("Quit Game");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}