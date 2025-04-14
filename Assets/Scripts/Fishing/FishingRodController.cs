using UnityEngine;

public class FishingRodController : MonoBehaviour
{
    public float rodEndurance = 10.0f;
    private float currentEndurance;
    private enum ActionState { Left, Right, Idle};
    private enum LineState { Tighten, Release, Idle};

    private ActionState actionState;
    private LineState lineState;

    private bool isFishing;
    private Transform fishTransform;

    [SerializeField] private ReelSpin reelSpin;

    [SerializeField]
    Transform xrOriginTransform;

    public HookThrower hookThrower;

    public float maxLineLength = 30.0f;
    public float minLineLength = 15.0f;
    public float hookRetrieveThreshold = 3.0f;
    public float lineMultiplier = 5.0f;
    private float currentLineLength;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        actionState = ActionState.Idle;
        lineState = LineState.Idle;
        isFishing = false;
        currentLineLength = minLineLength;
    }

    // Update is called once per frame
    void Update()
    {
        // need to compare fishing state
        if (!isFishing)
        {
            if (lineState != LineState.Idle)
            {
                hookThrower.SetLineLength(currentLineLength);
            }
            if (currentLineLength <= hookRetrieveThreshold)
            {
                hookThrower.RetrieveHook();
                hookThrower.SetLineLength(minLineLength);
                currentLineLength = minLineLength;
            }
            if (lineState == LineState.Tighten)
            {
                currentLineLength -= Time.deltaTime * lineMultiplier * reelSpin.GetSpinSpeed();

                //!isFishing &&
                if (hookThrower.GetHookState())
                {
                    currentLineLength = Mathf.Max(currentLineLength, hookRetrieveThreshold);
                }
                else currentLineLength = Mathf.Max(currentLineLength, minLineLength);
            }
            else if (lineState == LineState.Release)
            {
                currentLineLength += Time.deltaTime * lineMultiplier * reelSpin.GetSpinSpeed();
                currentLineLength = Mathf.Min(currentLineLength, maxLineLength);
            }
        }

        SetLineState();

        if (isFishing && fishTransform)
        {
            CalculateDirection();    
        }
    }

    public float GetEndurance()
    {
        return currentEndurance;
    }

    public void SetEndurance(float end)
    {
        currentEndurance = end;
    }

    public int[] GetOperationState()
    {
        return new int[] { (int)actionState, (int)lineState };
    }

    public float GetCurrentLineLength()
    {
        return currentLineLength;
    }

    public void SetFishTransform(Transform transform)
    {
        fishTransform = transform;
    }

    public void SetFishing(bool fishing)
    {
        isFishing = fishing;
        hookThrower.SetActive(!fishing);
        currentEndurance = rodEndurance;
    }

    void CalculateDirection() {
        Vector3 originToFish = fishTransform.position - xrOriginTransform.position;
        Vector3 rodToOrigin = transform.position - xrOriginTransform.position;

        // Plane normal: perpendicular to originToFish and world up
        Vector3 planeNormal = Vector3.Cross(originToFish.normalized, Vector3.up);

        // Dot product: sign tells you which side of the plane the rod is on
        float side = Vector3.Dot(rodToOrigin, planeNormal);

        if (side > 0.1f)
        {
            actionState = ActionState.Left;
            //Debug.Log("Rod is on the RIGHT side (intending to drag right)");
        }
        else if (side < -0.1f)
        {
            actionState = ActionState.Right;
            //Debug.Log("Rod is on the LEFT side (intending to drag left)");
        }
        else
        {
            actionState = ActionState.Idle;
            //Debug.Log("Rod is centered");
        }
    }

    void SetLineState() {
        lineState = (LineState)reelSpin.GetSpinDirection();
    }

}
