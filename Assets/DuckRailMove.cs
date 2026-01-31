using UnityEngine;

public class DuckRailMove : MonoBehaviour
{
    [Header("Path")]
    [SerializeField] private DuckPath path;

    [Header("Move")]
    [SerializeField] private float speed = 3f;

    [Header("Walk / Pause Ranges (seconds)")]
    [SerializeField] private Vector2 walkTimeRange = new Vector2(1.0f, 2.5f);
    [SerializeField] private Vector2 pauseTimeRange = new Vector2(0.3f, 1.2f);

    private bool _isWalking;
    private float _phaseTimer;

    private float _distance; // distancia recorrida sobre el path

    public void Initialize(DuckPath duckPath, float startT01 = 0f)
    {
        path = duckPath;

        if (path == null || path.Count < 2)
            return;

        path.RebuildCache();

        _distance = Mathf.Clamp01(startT01) * path.TotalLength;
        transform.position = path.GetPositionAtDistance(_distance);

        StartWalkingPhase();
    }

    private void Update()
    {
        if (path == null || path.Count < 2 || path.TotalLength <= 0f)
            return;

        _phaseTimer -= Time.deltaTime;
        if (_phaseTimer <= 0f)
        {
            if (_isWalking) StartPausePhase();
            else StartWalkingPhase();
        }

        if (_isWalking)
        {
            _distance += speed * Time.deltaTime;

            if (path.PathType == DuckPathType.Loop)
            {
                _distance = Mathf.Repeat(_distance, path.TotalLength);
                transform.position = path.GetPositionAtDistance(_distance);
            }
            else
            {
                if (_distance >= path.TotalLength)
                {
                    Destroy(gameObject);
                    return;
                }

                transform.position = path.GetPositionAtDistance(_distance);
            }
        }
    }

    private void StartWalkingPhase()
    {
        _isWalking = true;
        _phaseTimer = RandomRangeMin(pauseSafe: false, walkTimeRange);
    }

    private void StartPausePhase()
    {
        _isWalking = false;
        _phaseTimer = RandomRangeMin(pauseSafe: true, pauseTimeRange);
    }

    private float RandomRangeMin(bool pauseSafe, Vector2 range)
    {
        float min = Mathf.Min(range.x, range.y);
        float max = Mathf.Max(range.x, range.y);
        float v = Random.Range(min, max);
        return Mathf.Max(v, 0.05f);
    }
}
