using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.InputSystem;

public class HookThrower : MonoBehaviour
{
    [SerializeField] private GameObject hookPrefab;
    [SerializeField] private Transform hookSpawnPoint;

    private GameObject currentHook;
    private Rigidbody hookRigidbody;

    public float castMultiplier = 5f;         // Adjust this for casting strength
    public float swingThreshold = 3.0f;

    private Vector3 previousRodTipPosition;
    private bool hookReleased = false;

    public LineVisualizer lineVisualizer;

    void Start()
    {
        previousRodTipPosition = transform.position;
    }

    void Update()
    {
        Vector3 rodVelocity = CalculateVelocity();
        if(rodVelocity.magnitude > swingThreshold && !hookReleased)
        {
            Debug.Log(rodVelocity.magnitude);
            CastHook(rodVelocity);
        }
        // Update rod tip position for velocity calculation
        previousRodTipPosition = transform.position;
    }

    Vector3 CalculateVelocity()
    {
        return (transform.position - previousRodTipPosition) / Time.deltaTime;
    }

    void CastHook(Vector3 rodVelocity)
    {
        if (hookPrefab == null || hookSpawnPoint == null) return;

        // Instantiate hook at runtime
        currentHook = Instantiate(hookPrefab, hookSpawnPoint.position, Quaternion.identity);
        hookRigidbody = currentHook.GetComponent<Rigidbody>();

        if (hookRigidbody == null) return;

        hookRigidbody.isKinematic = false;
        hookRigidbody.useGravity = true;

        // Compute direction and apply force
        Vector3 directionToHook = (hookSpawnPoint.position - transform.position).normalized;
        Vector3 flingDirection = Vector3.ProjectOnPlane(rodVelocity, directionToHook).normalized;
        Vector3 force = castMultiplier * rodVelocity.magnitude * flingDirection;

        hookRigidbody.AddForce(force, ForceMode.Impulse);
        hookReleased = true;

        lineVisualizer.SetTransform(currentHook.transform);
    }

    public void RetrieveHook()
    {
        hookReleased = false;
        lineVisualizer.SetTransform(null);
        Destroy(currentHook);
    }

    public bool GetHookState()
    { 
        return hookReleased; 
    }
}
