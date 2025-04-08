using UnityEngine;

[RequireComponent(typeof(Renderer))] // 需要渲染组件
public class PureVerticalFloat : MonoBehaviour
{
    [Header("浮动设置")]
    public float floatHeight = 0.5f;   // 浮动幅度（米）
    public float floatSpeed = 1f;      // 浮动速度（周期/秒）

    [Header("发光效果")]
    public string glowProperty = "_EmissionColor"; // Shader发光属性名
    public Color glowColor = Color.cyan;          // 发光颜色
    public float maxGlowIntensity = 2f;           // 最大发光强度

    [Header("粒子效果")]
    public ParticleSystem rippleParticles;  // 水波纹粒子
    public float maxParticleRate = 15f;     // 最大粒子发射率

    private Vector3 baseYPosition;
    private float timer;
    private Material material;
    private float baseParticleRate;

    void Start()
    {
        // 记录初始Y轴位置（保持XZ不变）
        baseYPosition = transform.position;
        
        // 初始化材质
        material = GetComponent<Renderer>().material;
        
        // 初始化粒子
        if (rippleParticles != null)
        {
            baseParticleRate = rippleParticles.emission.rateOverTime.constant;
        }
    }

    void Update()
    {
        // 更新计时器（标准化到0-1）
        timer = Mathf.Repeat(timer + Time.deltaTime * floatSpeed, 1f);
        
        // 纯垂直浮动（使用平滑的Sin曲线）
        float verticalOffset = Mathf.Sin(timer * Mathf.PI * 2f) * floatHeight;
        transform.position = new Vector3(
            baseYPosition.x,
            baseYPosition.y + verticalOffset,
            baseYPosition.z
        );

        // 脉冲发光（强度随高度变化）
        float glowFactor = Mathf.Abs(Mathf.Sin(timer * Mathf.PI * 2f));
        material.SetColor(glowProperty, glowColor * (glowFactor * maxGlowIntensity));

        // 粒子效果（高点时增强）
        if (rippleParticles != null)
        {
            var emission = rippleParticles.emission;
            emission.rateOverTime = baseParticleRate + glowFactor * maxParticleRate;
        }
    }

    void OnDestroy()
    {
        // 清理实例化材质
        if (Application.isPlaying && material != null)
        {
            Destroy(material);
        }
    }

    // 编辑器可视化浮动范围
    void OnDrawGizmosSelected()
    {
        Vector3 center = Application.isPlaying ? baseYPosition : transform.position;
        Gizmos.color = new Color(0, 1, 1, 0.5f);
        Gizmos.DrawLine(
            center - Vector3.up * floatHeight,
            center + Vector3.up * floatHeight
        );
        Gizmos.DrawWireSphere(center, 0.1f);
    }
}