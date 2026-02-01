using System.Collections;
using UnityEngine;

public class DuckController : MonoBehaviour
{
    [Header("MASK REFERENCES")] [SerializeField]
    private GameObject[] masks;

    [SerializeField] private int objectiveMaskIndex = 0;

    [Header("GAMEPLAY")] [SerializeField] private bool isObjective = false;
    [SerializeField] private bool destroyOnObjectiveClick = true;

    [Header("RANDOM MASK SHOW")] [SerializeField, Range(0f, 1f)]
    private float chanceToShowMask = 0.6f;

    [SerializeField] private Vector2 randomOpenDelay = new Vector2(0.1f, 1.5f);
    [SerializeField] private Vector2 randomVisibleTime = new Vector2(0.8f, 2.5f);

    public bool IsObjective => isObjective;

    private GameObject currentMask;
    private int currentMaskIndex = -1;

    private bool clicked;
    private bool initialized;

    private Coroutine autoShowRoutine;

    // =========================
    // UNITY
    // =========================
    private void Awake()
    {
        // Apaga todas las máscaras
        if (masks == null) return;

        foreach (var m in masks)
        {
            if (m != null)
                m.SetActive(false);
        }
    }

    private void OnDisable()
    {
        if (autoShowRoutine != null)
            StopCoroutine(autoShowRoutine);
    }

    private void OnMouseDown()
    {
        if (clicked) return;
        clicked = true;

        Debug.Log($"Duck clicked: {name} | Objective={isObjective}");

        if (isObjective && GameplayManager.Instance != null)
            GameplayManager.Instance.OnDuckKilled(true);

        if (isObjective && destroyOnObjectiveClick)
            Destroy(gameObject);
    }

    // =========================
    // MASK SETUP
    // =========================
    private void Start()
    {
        autoShowRoutine = StartCoroutine(AutoShowMaskRoutine());
    }

    
    public void SetObjective(bool value)
    {
        isObjective = value;
        if (isObjective)
        {
            currentMaskIndex = objectiveMaskIndex;
        }
        else
        {
            currentMaskIndex = GetRandomMaskIndexExcludingObjective();
        }
            
    }

    private int GetRandomMaskIndexExcludingObjective()
    {
        if (masks.Length <= 1)
            return objectiveMaskIndex;

        int index;
        do
        {
            index = Random.Range(0, masks.Length);
        } while (index == objectiveMaskIndex);

        return index;
    }
  
    private IEnumerator AutoShowMaskRoutine()
    {
        while (!clicked)
        {
            float delay = Random.Range(randomOpenDelay.x, randomOpenDelay.y);
            yield return new WaitForSeconds(delay);

            if (clicked) yield break;
            if (Random.value > chanceToShowMask) continue;
            if (currentMaskIndex < 0 || currentMaskIndex >= masks.Length) continue;

            currentMask = masks[currentMaskIndex];
            if (currentMask == null) continue;

            currentMask.SetActive(true);

            float visibleTime = Random.Range(randomVisibleTime.x, randomVisibleTime.y);
            yield return new WaitForSeconds(visibleTime);

            if (currentMask != null)
                currentMask.SetActive(false);
        }
    }
    
}