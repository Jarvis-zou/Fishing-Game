using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class HookThrower : MonoBehaviour
{
    [SerializeField] private GameObject hookPrefab;
    [SerializeField] private Transform hookSpawnPoint;
    [SerializeField] private Transform cameraTransform;

    public RodGrabListener rodGrabListener;
    private GameObject currentHook;
    private Rigidbody hookRigidbody;

    public float castMultiplier = 5f;
    public float swingThreshold = 30.0f;

    private Vector3 previousRodTipPosition;
    private bool hookReleased = false;

    private float pullForce = 1.0f;

    private float lineLength = 15.0f;

    public LineVisualizer lineVisualizer;

    private bool active;

    void Start()
    {
        previousRodTipPosition = transform.position;
        active = true;
    }

    void Update()
    {
        if (!active) return;
        if(hookReleased)
        {
            float currentLength = Vector3.Distance(transform.position, currentHook.transform.position);
            if(currentLength >= lineLength)
            {
                PullHookBack(currentLength - lineLength);
            }
        }

        Vector3 rodVelocity = CalculateVelocity();
        if(rodVelocity.magnitude > swingThreshold && !hookReleased && rodGrabListener.GetIsGrabbed())
        {
            CastHook(rodVelocity);
        }
        previousRodTipPosition = transform.position;
    }


    Vector3 CalculateVelocity()
    {
        return (transform.position - previousRodTipPosition) / Time.deltaTime;
    }

    void CastHook(Vector3 rodVelocity)
    {
        if (hookPrefab == null || hookSpawnPoint == null) return;

        // Compute direction and apply force
        Vector3 directionToHook = (hookSpawnPoint.position - transform.position).normalized;
        Vector3 flingDirection = Vector3.ProjectOnPlane(rodVelocity, directionToHook).normalized;

        if (Vector3.Dot(flingDirection, cameraTransform.forward) < 0.5f) return;

        // Instantiate hook at runtime
        currentHook = Instantiate(hookPrefab, hookSpawnPoint.position, Quaternion.identity);
        hookRigidbody = currentHook.GetComponent<Rigidbody>();

        if (hookRigidbody == null) return;

        hookRigidbody.isKinematic = false;
        hookRigidbody.useGravity = true;

        Vector3 force = castMultiplier * rodVelocity.magnitude * flingDirection;

        hookRigidbody.AddForce(force, ForceMode.Impulse);
        hookReleased = true;

        lineVisualizer.SetTransform(currentHook.transform);
        SendHaptic();
    }

    public void RetrieveHook()
    {
        hookReleased = false;
        lineVisualizer.SetTransform(null);
        HookController hookController = currentHook.GetComponent<HookController>();
        if (hookController != null) { hookController.Clear(); }
        Destroy(currentHook);
    }

    public bool GetHookState()
    { 
        return hookReleased; 
    }

    public void SetLineLength(float length)
    {
        lineLength = length;
    }

    public void SetActive(bool act)
    {
        active = act;
    }

    public void PullHookBack(float distance)
    {
        Vector3 pullDir = (transform.position - currentHook.transform.position).normalized;
        Vector3 force = distance * pullForce * pullDir;
        Vector3 damp = hookRigidbody.linearVelocity * 0.1f;
        hookRigidbody.AddForce (force - damp, ForceMode.Impulse);
    }

    public GameObject GetCurrentHook()
    {
        return currentHook;
    }


    public XRNode xrNode = XRNode.RightHand;
    [Range(0, 1)] public float amplitude = 0.5f;
    public float duration = 0.1f;

    public void SendHaptic()
    {
        UnityEngine.XR.InputDevice device = InputDevices.GetDeviceAtXRNode(xrNode);
        if (device.isValid && device.TryGetHapticCapabilities(out var capabilities) && capabilities.supportsImpulse)
        {
            device.SendHapticImpulse(0, amplitude, duration);
        }
    }
}
