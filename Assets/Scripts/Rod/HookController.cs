using UnityEngine;

public class HookController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private GameObject fish;

    public void SetFish(GameObject obj)
    {
        fish = obj;

        fish.transform.SetParent(transform);

        fish.transform.localRotation = Quaternion.identity;
        fish.transform.localScale = fish.transform.localScale *0.2f;

        float yOffset = -fish.transform.localScale.y / 2f - 0.1f;
        fish.transform.localPosition = new Vector3(0, -yOffset, 0);
        Animator animator = fish.GetComponent<Animator>();
        if(animator != null ) animator.enabled = false;
    }   

    public bool HasFish()
    {
        return fish != null;
    }

    public void Clear() 
    {
        if(fish != null) Destroy(fish);
    }
}
