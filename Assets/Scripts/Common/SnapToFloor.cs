using UnityEditor;
using UnityEngine;

public class SnapToFloor
{
    [MenuItem("Tools/Snap to Floor %e")]
    public static void SnapSelectedToFloor()
    {
        var selectedObjects = Selection.gameObjects;

        if (selectedObjects.Length == 0)
        {
            Debug.Log("선택한 오브젝트 없음");
            return;
        }

        foreach (var go in selectedObjects)
        {
            Undo.RecordObject(go.transform, "Snap to Floor");

            var colliders = go.GetComponentsInChildren<Collider>();
            if (colliders.Length == 0)
            {
                if (Physics.Raycast(go.transform.position, Vector3.down, out RaycastHit hit, 1000f))
                {
                    go.transform.position = hit.point;
                }
                continue;
            }

            Bounds totalBounds = colliders[0].bounds;
            foreach (var collider in colliders)
            {
                totalBounds.Encapsulate(collider.bounds);
            }

            Vector3 castCenter = totalBounds.center + Vector3.up * 0.1f;
            Vector3 halfExtents = totalBounds.extents;

            if (Physics.BoxCast(castCenter, halfExtents, Vector3.down, out RaycastHit hitInfo, go.transform.rotation, 1000f))
            {
                float pivotToBottomDistance = go.transform.position.y - totalBounds.min.y;

                Vector3 newPosition = go.transform.position;
                newPosition.y = hitInfo.point.y + pivotToBottomDistance;

                go.transform.position = newPosition;
            }
        }
    }
}
