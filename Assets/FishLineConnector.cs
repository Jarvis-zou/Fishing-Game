using UnityEngine;

public class FishLineConnector : MonoBehaviour
{
    [Header("Exterior")]
    public Transform rodTip;      // Rod Top Point
    public Transform target;      // Fish/Float

    [Header("Interior")]
    public Transform lineStart;   // FishLine -> Rod Top
    public Transform lineEnd;     // FishLine -> Float/Fish
    public Transform lineBody;    // FishLine
    void Update()
    {
        if (rodTip == null || target == null || lineStart == null || lineEnd == null || lineBody == null)
            return;

        // Connect fishline with rod and target
        lineStart.position = rodTip.position;
        lineEnd.position = target.position;

        // Get fishline model size
        Vector3 start = lineStart.position;
        Vector3 end = lineEnd.position;
        Vector3 direction = end - start;
        float distance = direction.magnitude;

        // Set mid point
        lineBody.position = (start + end) / 2f;

        // Set Rotation
        lineBody.rotation = Quaternion.FromToRotation(Vector3.up, direction);

        // Set Length
        Vector3 scale = lineBody.localScale;
        scale.y = distance / 2f;
        lineBody.localScale = scale;
    }
}
