using Unity.VisualScripting;
using UnityEngine;

public class HookController : MonoBehaviour
{
    private GameObject fish;

    public AudioClip splashSound;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void SetFish(GameObject obj)
    {
        fish = obj;

        float yOffset = 0.5f;
        fish.transform.position = new Vector3(transform.position.x, transform.position.y - yOffset, transform.position.z);
        
        fish.transform.SetParent(transform);

        fish.transform.localRotation = Quaternion.identity;
        fish.transform.localScale = fish.transform.localScale * 0.5f;
        Animator animator = fish.GetComponent<Animator>();
        if (animator != null) animator.enabled = false;
    }

    public bool HasFish()
    {
        return fish != null;
    }

    public void Clear()
    {
        if (fish != null) Destroy(fish);
    }

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (other.gameObject.CompareTag("Water") && rb.linearVelocity.magnitude >= 3.0f)
        {
            audioSource.PlayOneShot(splashSound);
        }
    }

}