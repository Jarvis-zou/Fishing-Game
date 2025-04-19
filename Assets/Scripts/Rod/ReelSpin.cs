using UnityEngine;
using UnityEngine.InputSystem;

public class ReelSpin : MonoBehaviour
{
    [SerializeField] private Transform controller;
    [SerializeField] private Transform spinRef;
    [SerializeField] private Transform reelKnob;
    [SerializeField] private float interactionRadius = 0.1f;
    [SerializeField] private float rotationMultiplier = 1.0f;

    [SerializeField] private InputActionReference triggerAction;

    private ReelAudioController reelAudioController;
    private bool isInteracting = false;
    private Vector3 lastPosition;
    private enum SpinDirectionState { Clockwise, CounterClockwise, Idle};
    private SpinDirectionState spinDirection;
    private float spinSpeed = 1.0f;
    private float lastAngle = 0.0f;

    void Start()
    {
        if(controller == null)
        {
            Debug.Log("Controller Transform not assigned for ReelSpin");
        }
        if (spinRef == null)
        {
            Debug.Log("spinRef Transform not assigned for ReelSpin");
        }
        if (reelKnob == null)
        {
            Debug.Log("reelKnob Transform not assigned for ReelSpin");
        }
        if (triggerAction != null)
        {
            triggerAction.action.performed += OnTriggerPerformed;
            triggerAction.action.canceled += OnTriggerCanceled;
            spinDirection = SpinDirectionState.Idle;
            lastPosition = transform.position;
            reelAudioController = GetComponent<ReelAudioController>();
        }
    }

    void OnDestroy()
    {
        if (triggerAction != null)
        {
            triggerAction.action.performed -= OnTriggerPerformed;
            triggerAction.action.canceled -= OnTriggerCanceled;
        }
    }

    void Update()
    {
        if (controller == null) return;

        float distance = Vector3.Distance(controller.position, transform.position);

        if (isInteracting && distance < interactionRadius * 1.5f)
        {
            RotateReelBasedOnControllerMovement();
            if (reelAudioController != null)
            {
                reelAudioController.SetSpinState((int)spinDirection);
            }
        }
        else if (isInteracting)
        {
            EndInteraction();
        }
    }

    private void OnTriggerPerformed(InputAction.CallbackContext context)
    {
        if (controller == null) return;

        float distance = Vector3.Distance(controller.position, reelKnob.position);
        if (distance < interactionRadius && !isInteracting)
        {
            StartInteraction();
        }
    }

    private void OnTriggerCanceled(InputAction.CallbackContext context)
    {
        if (isInteracting)
        {
            EndInteraction();
            spinDirection = SpinDirectionState.Idle;
        }
    }

    void StartInteraction()
    {
        isInteracting = true;
        lastPosition = controller.position;

        reelKnob.GetComponent<Renderer>().material.color = Color.yellow;
    }

    void EndInteraction()
    {
        isInteracting = false;
        reelAudioController.SetSpinState(2);

        reelKnob.GetComponent<Renderer>().material.color = Color.white;
    }

    public int GetSpinDirection() { 
        return (int)spinDirection;
    }

    public float GetSpinSpeed()
    { return spinSpeed; }
    void RotateReelBasedOnControllerMovement()
    {
        Vector3 currentPosition = controller.position;

        // reel's coordinate system
        Vector3 reelForward = transform.forward;
        Vector3 reelUp = spinRef.up;

        // get vector from reel knob to controller
        Vector3 reelToController = currentPosition - spinRef.position;
        Vector3 reelToLast = lastPosition - spinRef.position;

        // project controller position onto the plane perpendicular to rotation axis
        Vector3 projectedControllerVector = Vector3.ProjectOnPlane(reelToController, reelForward);
        Vector3 lastProjected = Vector3.ProjectOnPlane(reelToLast, reelForward).normalized;

        // angle between the projected vector and reelUp vector
        float angle = Vector3.SignedAngle(reelUp, projectedControllerVector, reelForward);
        float direction = Vector3.Dot(Vector3.Cross(lastProjected, projectedControllerVector), reelForward);

        spinSpeed = Mathf.Min(5.0f, Mathf.Abs(angle - lastAngle) * 3.14f / 180 / Time.deltaTime);

        if (Mathf.Abs(direction) < float.Epsilon)
        {
            spinDirection = SpinDirectionState.Idle;
        }
        else if(direction < 0) {
            spinDirection = SpinDirectionState.Clockwise;
        }
        else {
            spinDirection = SpinDirectionState.CounterClockwise;
        }

        // Get current rotation of knob around forward axis
        Quaternion currentRotation = transform.rotation;
        Vector3 currentEuler = currentRotation.eulerAngles;

        // Set the rotation directly to match controller position
        transform.rotation = Quaternion.Euler(
            currentEuler.x,
            currentEuler.y,
            angle
        );


        lastPosition = currentPosition;
        lastAngle = angle;
    }
}
