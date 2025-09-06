using System.Collections.Generic;
using UnityEngine;

public static class GameUtils
{
    public static Vector3 GetSlopeJumpVector(Vector3 groundNormal, float upFactor, float jumpForce)
    {
        float normalY = groundNormal.y;
        float upCompensation = normalY <= 1f ? 0.5f : 0;
        Vector3 verticalDirection = Vector3.up * ((normalY + upCompensation) * upFactor);
        Vector3 horizontalDirection = groundNormal;
        Vector3 jumpVector = (horizontalDirection + verticalDirection).normalized * jumpForce;
        
        return jumpVector;
    }
    
    public static Vector3 RandomPointInCube(Vector3 center, Vector3 size)
    {
        return center + new Vector3(
            Random.Range(-size.x * 0.5f, size.x * 0.5f),
            Random.Range(-size.y * 0.5f, size.y * 0.5f),
            Random.Range(-size.z * 0.5f, size.z * 0.5f)
        );
    }
    
    public static List<(Vector3 position, Vector3 direction)> CreateOutwardPointsSphere(Vector3 center, float radius, int numPoints)
    {
        var points = new List<(Vector3, Vector3)>(numPoints);

        float offset = 2f / numPoints;
        float increment = Mathf.PI * (3f - Mathf.Sqrt(5f));

        for (int i = 0; i < numPoints; i++)
        {
            float y = i * offset - 1f + (offset / 2f);
            float r = Mathf.Sqrt(1f - y * y);
            float phi = i * increment;

            float x = Mathf.Cos(phi) * r;
            float z = Mathf.Sin(phi) * r;

            Vector3 dir = new Vector3(x, y, z).normalized;
            Vector3 pos = center + dir * radius;

            points.Add((pos, dir));
        }

        return points;
    }
}
