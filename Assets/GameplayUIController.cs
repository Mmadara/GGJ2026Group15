using UnityEngine;

public class GameplayUIController : MonoBehaviour
{
    [Header("UI Screens")]
    [SerializeField] private GameObject winUI;
    [SerializeField] private GameObject loseUI;

    private GameStateType _lastState = GameStateType.None;

    private void Start()
    {
        if (winUI != null) winUI.SetActive(false);
        if (loseUI != null) loseUI.SetActive(false);
    }

    private void Update()
    {
        if (GameplayManager.Instance == null)
            return;

        GameStateType currentState = GameplayManager.Instance.State;

        if (currentState == _lastState)
            return;

        _lastState = currentState;

        switch (currentState)
        {
            case GameStateType.Win:
                ShowWin();
                break;

            case GameStateType.Lose:
                ShowLose();
                break;
        }
    }

    private void ShowWin()
    {
        if (winUI != null)
            winUI.SetActive(true);

        if (loseUI != null)
            loseUI.SetActive(false);
    }

    private void ShowLose()
    {
        if (loseUI != null)
            loseUI.SetActive(true);

        if (winUI != null)
            winUI.SetActive(false);
    }
}