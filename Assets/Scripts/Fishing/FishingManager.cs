using UnityEngine;

public class FishingManager : MonoBehaviour
{
    // Singleton instance
    public static FishingManager Instance { get; private set; }

    [SerializeField] private FishingRodController rodController;
    private IFishable currentFish;

    private enum FishingState { Idle, Casting, Hooked, Fighting, Caught, Failed }
    private FishingState currentState = FishingState.Idle;

    // Set up the instance
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject); // Prevent duplicates
            return;
        }

        Instance = this;
    }

    void Start()
    {
        // Maybe register events from rod and fish
    }

    public void SetCurrentFish(IFishable fish)
    {
        currentFish = fish;
        currentState = FishingState.Hooked;
        // Change FSM state, UI update, etc.
    }

    public void OnCatchSuccess()
    {
        currentState = FishingState.Caught;
        // Trigger animation, show UI, etc.
    }

    public void OnCatchFail()
    {
        currentState = FishingState.Failed;
        // Reset or trigger feedback
    }
}