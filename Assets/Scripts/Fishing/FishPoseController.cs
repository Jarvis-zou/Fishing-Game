using UnityEngine;
using static UnityEngine.UI.Image;

public class FishPoseController : MonoBehaviour
{
    private FishController fishController;
    private Transform refTransform;
    private Vector3 refVector;
    private float rotationSpeed = 3.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fishController = GetComponent<FishController>();
        FishingRodController fishingRodController = FishingManager.Instance.GetFishingRodController();
        refTransform = fishingRodController.GetXROriginTransform();
    }

    // Update is called once per frame
    void Update()
    {
        if(refTransform == null) return;
        refVector = transform.position - refTransform.position;

        int[] fishState = fishController.GetFishState();
        Vector3 targetDirection = GetTargetDirection(fishState[0], fishState[1]);
        //Debug.Log(targetDirection);
        RotateToward(targetDirection);
    }

    Vector3 GetTargetDirection(int directionState, int isFighting)
    {
        if (directionState == 0)
        {
            Vector3 dir270 = GetDirectionByAngle(270);
            if (isFighting == 0) return dir270;
            else
            {
                Vector3 combined = dir270 + new Vector3(refVector.x, 0f, refVector.z).normalized;
                return combined.normalized;
            }
        }
        else if (directionState == 1)
        {
            Vector3 dir90 = GetDirectionByAngle(90);
            if (isFighting == 0) return dir90;
            else
            {
                Vector3 combined = dir90 + new Vector3(refVector.x, 0f, refVector.z).normalized;
                return combined.normalized;
            }
        }
        else
        {
            if (isFighting == 0) return -new Vector3(refVector.x, 0f, refVector.z).normalized;
            else return new Vector3(refVector.x, 0f, refVector.z).normalized;
        }
    }

    Vector3 GetDirectionByAngle(float angleInDegrees)
    {
        float radians = angleInDegrees * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(radians), 0f, Mathf.Cos(radians)).normalized;
    }

    void RotateToward(Vector3 targetDirection)
    {
        transform.rotation = Quaternion.LookRotation(targetDirection, Vector3.up);
    }
}
