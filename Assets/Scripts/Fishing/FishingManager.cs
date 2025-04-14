using UnityEngine;
using UnityEngine.Networking;

public class FishingManager : MonoBehaviour
{
    // Singleton instance
    public static FishingManager Instance { get; private set; }

    [SerializeField] private FishingRodController rodController;
    [SerializeField] private UIController uiController;
    private IFishable currentFish;

    private enum FishingState { Idle, Hooked, Caught, Failed }
    private FishingState currentState = FishingState.Idle;

    private float rodEndurance = 0;
    private float endurance = 0;

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
        rodEndurance = rodController.GetRodEndurance();
    }

    private void Update()
    {
        if (currentFish != null)
        {
            if (endurance <= 0)
            {
                OnCatchFail();
                return;
            }
            // find the rod
            int[] operations = rodController.GetOperationState();
            bool correct = currentFish.CheckInput(operations[0], operations[1] == 0);
            if (!correct)
            {
                endurance -= Time.deltaTime;
            }
        }
    }

    public void SetCurrentFish(IFishable fish)
    {
        currentFish = fish;
        currentState = FishingState.Hooked;
        // Change FSM state, UI update, etc.
        endurance = rodEndurance;
        rodController.SetFishing(true);
        rodController.SetFishTransform((currentFish as MonoBehaviour)?.transform);

        // Display game UI after hooked
        initUI(currentFish, rodController);
    }

    public IFishable GetCurrentFish()
    {
        return currentFish;
    }

    public int[] GetFishState()
    {
        if(currentFish == null) return null;
        return currentFish.GetFishState();
    }
    
    public float GetFishStamina()
    {
        if (currentFish == null) return 0;
        return currentFish.GetStamina();
    }

    public float GetEndurance()
    {
        return endurance;
    }

    public string GetManagerState()
    {
        return currentState.ToString();
    }

    public void OnCatchSuccess()
    {
        currentState = FishingState.Caught;
        currentFish = null;
        rodController.SetFishing(false);
        rodController.SetFishTransform(null);
        // Trigger animation, show UI, etc.
    }

    public void OnCatchFail()
    {
        currentState = FishingState.Failed;
        currentFish.OnEscaped();
        currentFish = null;
        rodController.SetFishing(false);
        rodController.SetFishTransform(null) ;
        // Reset or trigger feedback
    }

    public void initUI(IFishable fish, FishingRodController rodController)
    {
        uiController.initUI(fish, rodController);
        uiController.ShowUI();
    }
}