using System.Collections;
using UnityEngine;

public class DuckController : MonoBehaviour
{
    [Header("MASK REFERENCES")]
    [SerializeField] private GameObject[] masks;              // All masks
    [SerializeField] private int objectiveMaskIndex = 0;      // Objective mask index

    [Header("MASK TRANSITION")]
    [SerializeField] private float fadeDuration = 0.35f;

    [Header("GAMEPLAY")]
    [SerializeField] private bool isObjective = false;
    [SerializeField] private bool destroyOnObjectiveClick = true;

    public bool IsObjective => isObjective;

    private GameObject currentMask;
    private int currentMaskIndex = -1;
    private bool clicked;
    private Coroutine maskRoutine;

    // =========================
    // UNITY
    // =========================
    private void Awake()
    {
        SetupInitialMask();
    }

    private void OnMouseDown()
    {
        if (clicked) return;
        clicked = true;

        Debug.Log($"Duck clicked: {name} | Objective={isObjective}");

        if (isObjective && GameplayManager.Instance != null)
            GameplayManager.Instance.OnDuckKilled(true);

        if (destroyOnObjectiveClick)
            Destroy(gameObject);
    }

    // =========================
    // MASK LOGIC
    // =========================
    private void SetupInitialMask()
    {
        if (masks == null || masks.Length == 0)
        {
            Debug.LogError($"No masks assigned on {name}");
            return;
        }

        foreach (var m in masks)
            m.SetActive(false);

        // Start NON objective
        SetRandomMaskExcludingObjective();
    }

    /// <summary>
    /// Sets this duck as objective or normal
    /// </summary>
    public void SetObjective(bool value)
    {
        isObjective = value;

        if (isObjective)
        {
            ChangeMaskImmediate(objectiveMaskIndex);
        }
        else
        {
            SetRandomMaskExcludingObjective();
        }
    }

    /// <summary>
    /// Random mask EXCLUDING objective mask
    /// </summary>
    public void SetRandomMaskExcludingObjective()
    {
        if (isObjective) return;
        if (masks.Length <= 1) return;

        int newIndex;
        int safety = 0;

        do
        {
            newIndex = Random.Range(0, masks.Length);
            safety++;
        }
        while (
            (newIndex == objectiveMaskIndex || newIndex == currentMaskIndex)
            && safety < 20
        );

        ChangeMaskSmooth(newIndex);
    }

    // =========================
    // INTERNAL MASK CONTROL
    // =========================
    private void ChangeMaskSmooth(int newIndex)
    {
        if (newIndex < 0 || newIndex >= masks.Length) return;

        if (maskRoutine != null)
            StopCoroutine(maskRoutine);

        maskRoutine = StartCoroutine(ChangeMaskRoutine(newIndex));
    }

    private void ChangeMaskImmediate(int newIndex)
    {
        if (maskRoutine != null)
            StopCoroutine(maskRoutine);

        if (currentMask != null)
            currentMask.SetActive(false);

        currentMaskIndex = newIndex;
        currentMask = masks[newIndex];
        currentMask.SetActive(true);
    }

    private IEnumerator ChangeMaskRoutine(int newIndex)
    {
        if (currentMask != null)
        {
            yield return Fade(currentMask, 1f, 0f);
            currentMask.SetActive(false);
        }

        currentMaskIndex = newIndex;
        currentMask = masks[newIndex];
        currentMask.SetActive(true);

        yield return Fade(currentMask, 0f, 1f);
    }

    private IEnumerator Fade(GameObject obj, float from, float to)
    {
        CanvasGroup cg = obj.GetComponent<CanvasGroup>();
        if (cg == null)
            cg = obj.AddComponent<CanvasGroup>();

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            cg.alpha = Mathf.Lerp(from, to, t / fadeDuration);
            yield return null;
        }

        cg.alpha = to;
    }
}
