using UnityEngine;

public class FishSwim_Sardine : MonoBehaviour
{
    [Header("Movement Settings")]
    public Transform poolCenter;      // Reference to the pool's center transform
    public float speed = 10f;         // Swimming speed (degrees per second)
    public float swimRadius = 7.5f;   // Swimming radius
    
    [Header("Depth Settings")]
    public float depthOffset = -0.3f; // How much below pool center (negative = underwater)
    public bool randomizeDepth = true;// Enable random depth variation
    public float minDepth = -0.5f;    // Minimum depth offset
    public float maxDepth = -0.1f;    // Maximum depth offset

    private float angle;
    private float currentDepth;
    private Vector3 currentPoolPosition;

    void Start()
    {
        // Initialize with current pool position
        currentPoolPosition = poolCenter ? poolCenter.position : Vector3.zero;
        
        // Random starting angle
        angle = Random.Range(0f, 360f);
        
        // Set random depth if enabled
        currentDepth = randomizeDepth ? 
            Random.Range(minDepth, maxDepth) : depthOffset;
        
        UpdatePosition();
        UpdateRotation();
    }

    void Update()
    {
        // Update pool position if reference exists
        if (poolCenter) currentPoolPosition = poolCenter.position;
        
        // Update movement
        angle -= speed * Time.deltaTime;
        angle = Mathf.Repeat(angle, 360f);
        
        UpdatePosition();
        UpdateRotation();
    }

    void UpdatePosition()
    {
        // Calculate circular position (relative to pool)
        float x = Mathf.Cos(angle * Mathf.Deg2Rad) * swimRadius;
        float z = Mathf.Sin(angle * Mathf.Deg2Rad) * swimRadius;
        
        // Apply position with depth offset
        transform.position = new Vector3(
            currentPoolPosition.x + x,
            currentPoolPosition.y + currentDepth,
            currentPoolPosition.z + z
        );
    }

    void UpdateRotation()
    {
        // Calculate tangent direction (clockwise movement)
        Vector3 tangentDirection = new Vector3(
            Mathf.Sin(angle * Mathf.Deg2Rad),
            0,
            -Mathf.Cos(angle * Mathf.Deg2Rad)
        ).normalized;

        // Apply rotation with slight downward tilt for realism
        if (tangentDirection != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(tangentDirection);
            
            // Optional: Add slight downward tilt (5 degrees)
            rotation *= Quaternion.Euler(5, 0, 0);
            
            transform.rotation = rotation;
        }
    }

    // Editor visualization
    void OnDrawGizmosSelected()
    {
        if (!poolCenter) return;
        
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(poolCenter.position, swimRadius);
        Gizmos.DrawLine(poolCenter.position, transform.position);
    }
}





