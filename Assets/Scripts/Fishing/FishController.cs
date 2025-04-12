using UnityEngine;

public class FishController : MonoBehaviour, IFishable
{
    public void OnCaught()
    {
        FishingManager.Instance.OnCatchSuccess();
    }

    public void OnEscaped()
    {
        FishingManager.Instance.OnCatchFail();
    }

    public void OnHooked()
    {
        FishingManager.Instance.SetCurrentFish(this);
    }


    void Start()
    {
        
    }


    void Update()
    {
        
    }
}
