using UnityEngine;

public class FishSwim_Emper : MonoBehaviour
{
    [Header("Swim")]
    public Transform poolCenter;   
    public float swimSpeed = 10f;  
    public float swimRadius = 7.5f;  
    
    [Header("Depth")] 
    public float yOffset = -0.4f;   
    public bool randomizeDepth = true; 
    public float minDepth = -0.5f;  
    public float maxDepth = -0.3f;  

    private float angle;
    private float finalYOffset;
    private Vector3 currentCenter;

    void Start()
    {
        currentCenter = poolCenter ? poolCenter.position : Vector3.zero;
        
        angle = Random.Range(0f, 360f);
        
        finalYOffset = randomizeDepth ? 
            Random.Range(minDepth, maxDepth) : yOffset;
        
        UpdatePosition();
        UpdateRotation();
    }

    void Update()
    {
        if(poolCenter) currentCenter = poolCenter.position;
        
        angle -= swimSpeed * Time.deltaTime;
        angle = Mathf.Repeat(angle, 360f);
        
        UpdatePosition();
        UpdateRotation();
    }

    void UpdatePosition()
    {
        float x = Mathf.Cos(angle * Mathf.Deg2Rad) * swimRadius;
        float z = Mathf.Sin(angle * Mathf.Deg2Rad) * swimRadius;

        transform.position = new Vector3(
            currentCenter.x + x,
            currentCenter.y + finalYOffset,
            currentCenter.z + z
        );
    }

    void UpdateRotation()
    {
        Vector3 tangentDirection = new Vector3(
            -Mathf.Sin(angle * Mathf.Deg2Rad), 
            0,
            Mathf.Cos(angle * Mathf.Deg2Rad)
        ).normalized;

        if(tangentDirection != Vector3.zero)
        {
            Vector3 rotatedDirection = Quaternion.Euler(0, -90, 0) * tangentDirection;
            transform.rotation = Quaternion.LookRotation(rotatedDirection);
        }
    }

    void OnDrawGizmosSelected()
    {
        if(!poolCenter) return;
        
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(poolCenter.position, swimRadius);
    }
}
