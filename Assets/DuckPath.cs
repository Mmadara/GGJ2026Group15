using System.Collections.Generic;
using UnityEngine;

public enum DuckPathType
{
    Linear,
    Loop
}

public class DuckPath : MonoBehaviour
{
    [SerializeField] private DuckPathType pathType = DuckPathType.Linear;
    [SerializeField] private List<GameObject> points = new List<GameObject>();

    public DuckPathType PathType => pathType;
    public IReadOnlyList<GameObject> Points => points;
    public int Count => points.Count;

    private readonly List<float> _segmentLengths = new List<float>();
    public float TotalLength { get; private set; }

    public Vector3 GetPointPosition(int index)
    {
        return points[index].transform.position;
    }

    private void Awake()
    {
        RebuildCache();
    }

    private void OnValidate()
    {
        RebuildCache();
    }

    public void RebuildCache()
    {
        _segmentLengths.Clear();
        TotalLength = 0f;

        if (points == null || points.Count < 2)
            return;

        int last = points.Count - 1;

        for (int i = 0; i < last; i++)
        {
            if (points[i] == null || points[i + 1] == null)
            {
                _segmentLengths.Add(0f);
                continue;
            }

            float len = Vector3.Distance(points[i].transform.position, points[i + 1].transform.position);
            _segmentLengths.Add(len);
            TotalLength += len;
        }

        if (pathType == DuckPathType.Loop)
        {
            var first = points[0];
            var lastGo = points[last];

            if (first != null && lastGo != null)
            {
                float len = Vector3.Distance(lastGo.transform.position, first.transform.position);
                _segmentLengths.Add(len); // segmento de cierre
                TotalLength += len;
            }
            else
            {
                _segmentLengths.Add(0f);
            }
        }
    }

    public Vector3 GetPositionAtNormalized(float t01)
    {
        if (points == null || points.Count < 2 || TotalLength <= 0f)
            return transform.position;

        t01 = Mathf.Clamp01(t01);
        float distance = t01 * TotalLength;
        return GetPositionAtDistance(distance);
    }

    public Vector3 GetPositionAtDistance(float distance)
    {
        if (points == null || points.Count < 2 || TotalLength <= 0f)
            return transform.position;

        // Para loop, la distancia la manejaremos con Repeat en el mover.
        distance = Mathf.Clamp(distance, 0f, TotalLength);

        int lastPointIndex = points.Count - 1;
        int segmentCount = (pathType == DuckPathType.Loop) ? points.Count : points.Count - 1;

        float remaining = distance;

        for (int seg = 0; seg < segmentCount; seg++)
        {
            float segLen = _segmentLengths[seg];
            if (segLen <= 0f) continue;

            if (remaining > segLen)
            {
                remaining -= segLen;
                continue;
            }

            int aIndex = seg;
            int bIndex = (seg == points.Count - 1) ? 0 : (seg + 1);

            var a = points[aIndex];
            var b = points[bIndex];

            if (a == null || b == null) return transform.position;

            float u = remaining / segLen;
            return Vector3.Lerp(a.transform.position, b.transform.position, u);
        }

        // fallback
        var lastGo = points[lastPointIndex];
        return lastGo != null ? lastGo.transform.position : transform.position;
    }

    private void DrawPath()
    {
        if (points == null || points.Count < 2)
            return;

        for (int i = 0; i < points.Count - 1; i++)
        {
            if (points[i] == null || points[i + 1] == null)
                continue;

            Gizmos.DrawLine(points[i].transform.position, points[i + 1].transform.position);
        }

        if (pathType == DuckPathType.Loop)
        {
            var first = points[0];
            var last = points[points.Count - 1];

            if (first != null && last != null)
                Gizmos.DrawLine(last.transform.position, first.transform.position);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        DrawPath();
    }
}
