using UnityEngine;

public class BeamLine
{
    Vector3 _start;
    public Vector3 start => _start;

    Vector3 _end;
    public Vector3 end => _end;

    public void Set(Vector3 start, Vector3 end)
    {
        _start = start;
        _end = end;
    }

    public Vector3 GetAt(float value)
    {
        return Vector3.Lerp(_start, _end, value);
    }

    public Vector3 NearestPointToPoint(Vector3 point)
    {
        var value = ClosestPointToPointParameter(point);
        value = Mathf.Clamp01(value);

        return GetAt(value);
    }

    public float ClosestPointToPointParameter(Vector3 point)
    {
        var startPoint = point - start;
        var startEnd = end - start;

        var startEnd2 = Vector3.Dot(startEnd, startEnd);
        var startEnd_startPoint = Vector3.Dot(startEnd, startPoint);

        var value = startEnd_startPoint / startEnd2;

        return value;
    }
}