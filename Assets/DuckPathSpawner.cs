using UnityEngine;

public class DuckPathSpawner : MonoBehaviour
{
    [SerializeField] private DuckPath path;
    [SerializeField] private GameObject duckPrefab;

    [Header("Objective Spawn")] [SerializeField, Range(0f, 1f)]
    private float objectiveChance = 0.8f;

    [Header("Linear Spawn")] [SerializeField]
    private float spawnInterval = 1.0f;

    [Header("Loop Population")] [SerializeField]
    private int loopDuckCount = 10;

    [SerializeField, Range(0f, 0.49f)] private float loopJitterPerDuck = 0.20f;

    private float _timer;

    private void Start()
    {
        if (path == null)
            path = GetComponent<DuckPath>();

        if (path == null || duckPrefab == null)
            return;

        path.RebuildCache();

        if (path.PathType == DuckPathType.Loop)
        {
            SpawnLoopPopulation();
        }
    }

    private void Update()
    {
        if (path == null || duckPrefab == null || path.Count < 2)
            return;

        if (path.PathType == DuckPathType.Loop)
            return; // en loop NO spawneamos infinito

        _timer += Time.deltaTime;
        if (_timer >= spawnInterval)
        {
            _timer = 0f;
            SpawnOneLinear();
        }
    }

    private void SpawnOneLinear()
    {
        Vector3 spawnPos = path.GetPointPosition(0);
        GameObject obj = Instantiate(duckPrefab, spawnPos, Quaternion.identity);

        bool isObjective = TryAssignObjective();

        DuckController duck = obj.GetComponent<DuckController>();
        if (duck != null)
        {
            duck.SetObjective(isObjective);
        }

        DuckRailMove mover = obj.GetComponent<DuckRailMove>();
        if (mover == null) mover = obj.AddComponent<DuckRailMove>();

        mover.Initialize(path, 0f);
    }

    private void SpawnLoopPopulation()
    {
        if (loopDuckCount <= 0) return;

        for (int i = 0; i < loopDuckCount; i++)
        {
            float baseT = (float)i / loopDuckCount;

            float slot = 1f / loopDuckCount;
            float jitter = Random.Range(-loopJitterPerDuck, loopJitterPerDuck) * slot;

            float t = Mathf.Repeat(baseT + jitter, 1f);

            Vector3 pos = path.GetPositionAtNormalized(t);
            GameObject obj = Instantiate(duckPrefab, pos, Quaternion.identity);

            bool isObjective = TryAssignObjective();

            DuckController duck = obj.GetComponent<DuckController>();
            if (duck != null)
            {
                duck.SetObjective(isObjective);
            }

            DuckRailMove mover = obj.GetComponent<DuckRailMove>();
            if (mover == null) mover = obj.AddComponent<DuckRailMove>();

            mover.Initialize(path, t);
        }
    }

    private bool TryAssignObjective()
    {
        if (objectiveChance <= 0f)
            return false;

        if (Random.value >= objectiveChance)
            return false;

        if (GameplayManager.Instance == null)
            return false;

        return GameplayManager.Instance.TryConsumeObjectiveSpawn();
    }
}