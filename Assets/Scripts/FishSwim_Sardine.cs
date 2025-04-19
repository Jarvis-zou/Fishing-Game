using UnityEngine;

public class FishSwim_Sardine : MonoBehaviour
{
    [Header("Movement Settings")]
    public Transform poolCenter;
    public float speed = 10f;    
    public float swimRadius = 7.5f;   
    
    [Header("Depth Settings")]
    public float depthOffset = -0.3f; 
    public bool randomizeDepth = true;
    public float minDepth = -0.5f;  
    public float maxDepth = -0.1f;    

    private float angle;
    private float currentDepth;
    private Vector3 currentPoolPosition;

    void Start()
    {
        currentPoolPosition = poolCenter ? poolCenter.position : Vector3.zero;

        angle = Random.Range(0f, 360f);

        currentDepth = randomizeDepth ? 
            Random.Range(minDepth, maxDepth) : depthOffset;
        
        UpdatePosition();
        UpdateRotation();
    }

    void Update()
    {
        if (poolCenter) currentPoolPosition = poolCenter.position;
        
        angle -= speed * Time.deltaTime;
        angle = Mathf.Repeat(angle, 360f);
        
        UpdatePosition();
        UpdateRotation();
    }

    void UpdatePosition()
    {
        float x = Mathf.Cos(angle * Mathf.Deg2Rad) * swimRadius;
        float z = Mathf.Sin(angle * Mathf.Deg2Rad) * swimRadius;
        
        transform.position = new Vector3(
            currentPoolPosition.x + x,
            currentPoolPosition.y + currentDepth,
            currentPoolPosition.z + z
        );
    }

    void UpdateRotation()
    {
        Vector3 tangentDirection = new Vector3(
            Mathf.Sin(angle * Mathf.Deg2Rad),
            0,
            -Mathf.Cos(angle * Mathf.Deg2Rad)
        ).normalized;

        // rotation with slight downward tilt for realism
        if (tangentDirection != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(tangentDirection);
            
            // slight downward tilt (5 degrees)
            rotation *= Quaternion.Euler(5, 0, 0);
            
            transform.rotation = rotation;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (!poolCenter) return;
        
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(poolCenter.position, swimRadius);
        Gizmos.DrawLine(poolCenter.position, transform.position);
    }
}





