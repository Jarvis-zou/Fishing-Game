using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineVisualizer : MonoBehaviour
{
    public Transform rodTip;
    private Transform hook;
    public float slack = 0.3f; // for a sagging curve effect

    private LineRenderer line;
    public float lineWidth = 0.02f;

    void Awake()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = 20;
        line.startWidth = lineWidth;
        line.endWidth = lineWidth;
    }

    void Update()
    {
        if (rodTip == null || hook == null)
        {
            if (line.positionCount != 0)
                line.positionCount = 0; // Clear the line when hook is null
            return;
        }

        if (line.positionCount == 0)
        {
            line.positionCount = 20; // Restore the default when hook is set again
            line.startWidth = lineWidth;
            line.endWidth = lineWidth;
        }

        Vector3 start = rodTip.position;
        Vector3 end = hook.position;

        // Fake a curve using interpolation + gravity sag
        for (int i = 0; i < line.positionCount; i++)
        {
            float t = i / (float)(line.positionCount - 1);
            Vector3 pos = Vector3.Lerp(start, end, t);
            pos.y -= Mathf.Sin(t * Mathf.PI) * slack;
            line.SetPosition(i, pos);
        }
    }

    public void SetTransform(Transform t)
    { 
        hook = t; 
    }

    public void SetSlack(float slack)
    {
        this.slack = slack;
    }

    public void ResetSlack()
    {
        slack = 0.3f;
    }

}
