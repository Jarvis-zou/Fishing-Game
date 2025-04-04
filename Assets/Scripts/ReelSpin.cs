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


    private bool isInteracting = false;
    private Vector3 lastPosition;

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

        // Check if controller is nearby
        float distance = Vector3.Distance(controller.position, transform.position);

        // Handle ongoing interaction
        if (isInteracting && distance < interactionRadius * 1.5f) // Give a bit more leeway once interaction starts
        {
            RotateReelBasedOnControllerMovement();
        }
        else if (isInteracting)
        {
            // Stop interaction if controller moves too far away
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
        }
    }

    void StartInteraction()
    {
        isInteracting = true;
        lastPosition = controller.position;

        // Optional visual feedback
        // reelKnob.GetComponent<Renderer>().material.color = Color.yellow;
    }

    void EndInteraction()
    {
        isInteracting = false;

        // Optional visual feedback
        // reelKnob.GetComponent<Renderer>().material.color = Color.white;
    }

    void RotateReelBasedOnControllerMovement()
    {
        // Current controller position
        Vector3 currentPosition = controller.position;

        // Define reel's coordinate system
        Vector3 reelForward = transform.forward; // Rotation axis
        Vector3 reelUp = spinRef.up;

        // Get vector from reel knob to controller
        Vector3 reelToController = currentPosition - spinRef.position;

        // Project controller position onto the plane perpendicular to rotation axis
        Vector3 projectedControllerVector = Vector3.ProjectOnPlane(reelToController, reelForward);

        // Calculate angle between the projected vector and reelUp vector
        float angle = Vector3.SignedAngle(reelUp, projectedControllerVector, reelForward);

        // Get current rotation of knob around forward axis
        Quaternion currentRotation = transform.rotation;
        Vector3 currentEuler = currentRotation.eulerAngles;

        // Set the rotation directly to match controller position
        transform.rotation = Quaternion.Euler(
            currentEuler.x,
            currentEuler.y,
            angle
        );

        // Apply haptic feedback based on rotation amount
        //if (useHaptics && xrController != null && Mathf.Abs(rotationDelta) > 1.0f)
        //{
        //    float hapticStrength = Mathf.Clamp01(Mathf.Abs(rotationDelta) / 10f) * hapticIntensity;
        //    xrController.SendHapticImpulse(hapticStrength, hapticDuration);
        //}

        // Update for next frame
        lastPosition = currentPosition;
    }
}
