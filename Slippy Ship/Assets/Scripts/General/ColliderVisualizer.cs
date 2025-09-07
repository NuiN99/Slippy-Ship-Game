using Unity.VisualScripting;
using UnityEngine;

public class ColliderVisualizer : MonoBehaviour
{
#if UNITY_EDITOR

    void OnDrawGizmos()
    {
        BoxCollider[] colliders = GetComponentsInChildren<BoxCollider>();

        Gizmos.color = Color.green.WithAlpha(0.25f);

        foreach (var col in colliders)
        {
            if (!col.enabled) continue;

            Matrix4x4 oldMatrix = Gizmos.matrix;

            Gizmos.matrix = col.transform.localToWorldMatrix;

            Gizmos.DrawCube(col.center, col.size);

            Gizmos.matrix = oldMatrix;
        }
    }
#endif
}