using UnityEngine;

public class DuckController : MonoBehaviour
{
    [SerializeField] private DuckAnimationController _duckAnimatorController;
    [SerializeField] private DuckAppearanceController _duckAppearanceController;

    [Header("Gameplay")] [SerializeField] private bool isObjective = false;
    [SerializeField] private bool destroyOnObjectiveClick = true;

    public bool IsObjective => isObjective;

    public void SetObjective(bool value)
    {
        isObjective = value;
    }

    private bool _clicked;

    private void OnMouseDown()
    {
        Debug.Log($"Duck clicked: {name} objective={isObjective}", this);
        if (_clicked) return;
        _clicked = true;

        if (isObjective && GameplayManager.Instance != null)
        {
            GameplayManager.Instance.OnDuckKilled(true);
        }

        Destroy(gameObject);
    }
}