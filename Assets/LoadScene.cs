using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScene : MonoBehaviour
{
    [SerializeField] private float delay = 1f;
    [SerializeField] private int sceneIndex = 1;
    [SerializeField] private Button button;

    private bool _hasBeenPressed;

    public void ButtonOnClick()
    {
        if (_hasBeenPressed)
            return;

        _hasBeenPressed = true;
        button.interactable = false;

        StartCoroutine(LoadSceneWithDelay());
    }

    private IEnumerator LoadSceneWithDelay()
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneIndex);
    }
}