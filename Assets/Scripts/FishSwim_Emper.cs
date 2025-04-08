using UnityEngine;

public class FishSwim_Emper : MonoBehaviour
{
    [Header("运动设置")]
    public Transform poolCenter;      // 泳池中心位置
    public float swimSpeed = 10f;     // 游泳速度（角度/秒）
    public float swimRadius = 7.5f;   // 游泳半径
    
    [Header("深度设置")] 
    public float yOffset = -0.4f;    // Y轴偏移量（模拟水下深度）
    public bool randomizeDepth = true; // 是否随机深度
    public float minDepth = -0.5f;    // 最小深度
    public float maxDepth = -0.3f;    // 最大深度

    private float angle;
    private float finalYOffset;
    private Vector3 currentCenter;

    void Start()
    {
        // 如果没有指定泳池中心，使用世界原点
        currentCenter = poolCenter ? poolCenter.position : Vector3.zero;
        
        // 初始化角度
        angle = Random.Range(0f, 360f);
        
        // 设置Y轴偏移
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
        
        // 应用位置时加入Y轴偏移
        transform.position = new Vector3(
            currentCenter.x + x,
            currentCenter.y + finalYOffset,
            currentCenter.z + z
        );
    }

    void UpdateRotation()
    {
        // 计算运动方向（圆形的切线方向）
        Vector3 tangentDirection = new Vector3(
            -Mathf.Sin(angle * Mathf.Deg2Rad),  // 顺时针切线X分量
            0,
            Mathf.Cos(angle * Mathf.Deg2Rad)     // 顺时针切线Z分量
        ).normalized;

        // 让鱼头朝向运动方向
        if(tangentDirection != Vector3.zero)
        {
            Vector3 rotatedDirection = Quaternion.Euler(0, -90, 0) * tangentDirection;
            transform.rotation = Quaternion.LookRotation(rotatedDirection);
        }
    }

    // 在编辑器中可视化运动路径
    void OnDrawGizmosSelected()
    {
        if(!poolCenter) return;
        
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(poolCenter.position, swimRadius);
    }
}
