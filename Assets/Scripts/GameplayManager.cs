using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager Instance { get; private set; }

    [Header("Ducks")]
    [SerializeField] private List<ScriptableDuckAppearance> _ducksAppearancesList =
        new List<ScriptableDuckAppearance>();

    [Header("Win Condition")]
    [SerializeField] private int ducksToKill = 10;

    [Header("Objective Budget")]
    [SerializeField] private int totalObjectivesToSpawn = 10;
    private ramdomenum _events;

    public int DucksRemaining { get; private set; }
    public int ObjectivesLeftToSpawn { get; private set; }

    public GameStateType State { get; private set; } = GameStateType.None;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        _events = GetComponent<ramdomenum>();
        StartGame();
    }

    private void StartGame()
    {
        DucksRemaining = ducksToKill;
        ObjectivesLeftToSpawn = totalObjectivesToSpawn;
        State = GameStateType.Playing;
        _events.IniciarEventos();

        Debug.Log($"Game Started | Kill:{DucksRemaining} | SpawnBudget:{ObjectivesLeftToSpawn}");
    }

    private void WinGame()
    {
        State = GameStateType.Win;
        _events.DetenerEventos();
        Debug.Log("YOU WIN");
    }

    // =========================
    // SPAWN BUDGET (SPAWNERS)
    // =========================

    public bool TryConsumeObjectiveSpawn()
    {
        if (State != GameStateType.Playing)
            return false;

        if (ObjectivesLeftToSpawn <= 0)
            return false;

        ObjectivesLeftToSpawn--;
        return true;
    }

    // =========================
    // EVENTS (DUCKS)
    // =========================

    public void OnDuckKilled(bool wasObjective)
    {
        if (State != GameStateType.Playing)
            return;

        if (!wasObjective)
            return;

        DucksRemaining--;

        Debug.Log($"Objective killed. Remaining:{DucksRemaining}");

        if (DucksRemaining <= 0)
        {
            WinGame();
        }
    }
}
