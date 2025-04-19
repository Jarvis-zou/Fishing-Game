using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class PureVerticalFloat : MonoBehaviour
{
    [Header("Float Setting")]
    public float floatHeight = 0.5f;
    public float floatSpeed = 1f; 

    [Header("Emission Effect")]
    public string glowProperty = "_EmissionColor"; 
    public Color glowColor = Color.cyan;         
    public float maxGlowIntensity = 2f;          

    [Header("Particle Effect")]
    public ParticleSystem rippleParticles; 
    public float maxParticleRate = 15f; 

    private Vector3 baseYPosition;
    private float timer;
    private Material material;
    private float baseParticleRate;

    void Start()
    {
        baseYPosition = transform.position;
        
        material = GetComponent<Renderer>().material;
        
        if (rippleParticles != null)
        {
            baseParticleRate = rippleParticles.emission.rateOverTime.constant;
        }
    }

    void Update()
    {
        timer = Mathf.Repeat(timer + Time.deltaTime * floatSpeed, 1f);
        
        float verticalOffset = Mathf.Sin(timer * Mathf.PI * 2f) * floatHeight;
        transform.position = new Vector3(
            baseYPosition.x,
            baseYPosition.y + verticalOffset,
            baseYPosition.z
        );

        float glowFactor = Mathf.Abs(Mathf.Sin(timer * Mathf.PI * 2f));
        material.SetColor(glowProperty, glowColor * (glowFactor * maxGlowIntensity));

        if (rippleParticles != null)
        {
            var emission = rippleParticles.emission;
            emission.rateOverTime = baseParticleRate + glowFactor * maxParticleRate;
        }
    }

    void OnDestroy()
    {
        if (Application.isPlaying && material != null)
        {
            Destroy(material);
        }
    }

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