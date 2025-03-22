using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Transform start; // Điểm bắt đầu
    public Transform end;   // Điểm kết thúc
    public float height = 3; // Độ cao cực đại của quỹ đạo
    public int resolution = 30; // Số điểm để vẽ đường

    void OnDrawGizmos()
    {
        if (start == null || end == null) return;

        Vector3 prevPoint = start.position;
        Gizmos.color = Color.cyan;

        for (int i = 1; i <= resolution; i++)
        {
            float t = (float)i / resolution;
            Vector3 newPoint = GetParabolaPoint(start.position, end.position, height, t);
            Gizmos.DrawLine(prevPoint, newPoint);
            prevPoint = newPoint;
        }
    }

    Vector3 GetParabolaPoint(Vector3 start, Vector3 end, float height, float t)
    {
        float x = Mathf.Lerp(start.x, end.x, t);
        float z = Mathf.Lerp(start.z, end.z, t);

        float y = Mathf.Lerp(start.y, end.y, t) + height * Mathf.Sin(t * Mathf.PI);

        return new Vector3(x, y, z);
    }
}
