using UnityEngine;

public class FishingManager : MonoBehaviour
{
    // Singleton instance
    public static FishingManager Instance { get; private set; }

    [SerializeField] private FishingRodController rodController;
    [SerializeField] private RodBendController bendController;

    [SerializeField] private UIController uiController;

    private IFishable currentFish;

    private enum FishingState { Idle, Hooked, Caught, Failed }
    private FishingState currentState = FishingState.Idle;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject); 
            return;
        }

        Instance = this;
    }

    void Start()
    {
    }

    private void Update()
    {
        if (currentFish != null)
        {
            float endurance = rodController.GetEndurance();
            if (endurance <= 0)
            {
                OnCatchFail();
                return;
            }
            int[] operations = rodController.GetOperationState();
            bool correct = currentFish.CheckInput(operations[0], operations[1] == 0);
            if (!correct)
            {
                rodController.SetEndurance(endurance - Time.deltaTime);
            }


            int[] fishState = currentFish.GetFishState();
            if (fishState[1] == 1)
            {
                bendController.SetIsFighting(true);
            }
            else
            {
                bendController.SetIsFighting(false);
            }
        }
    }

    public void SetCurrentFish(IFishable fish)
    {
        currentFish = fish;
        currentState = FishingState.Hooked;
        rodController.SetFishing(true);
        rodController.SetFishTransform((currentFish as MonoBehaviour)?.transform);

        if (bendController)
        {
            bendController.SetHooked(true);
        }

        InitUI(fish, rodController);
    }

    public FishingRodController GetFishingRodController()
    {
        return rodController;
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
        return rodController.GetEndurance();
    }

    public string GetManagerState()
    {
        return currentState.ToString();
    }

    public void OnCatchSuccess()
    {
        currentState = FishingState.Caught;
        
        rodController.SetFishing(false);
        rodController.SetFishTransform(null);
        GameObject fishCaughtPrefab = currentFish.GetCaughtFishPrefab();
        if(fishCaughtPrefab != null)
        {
            GameObject fishCaught = Instantiate(fishCaughtPrefab);
            rodController.SetFishCaught(fishCaught);
        }
        
        if (bendController)
        {
            bendController.SetHooked(false);
        }
        uiController.HideUI();
        currentFish = null;
    }

    public void OnCatchFail()
    {
        currentState = FishingState.Failed;
        currentFish.OnEscaped();
        currentFish = null;
        rodController.SetFishing(false);
        rodController.SetFishTransform(null);
        if (bendController)
        {
            bendController.SetHooked(false);
        }

        uiController.HideUI();
    }

    public void InitUI(IFishable fish, FishingRodController rodController)
    {
        uiController.initUI(fish, rodController);
        uiController.ShowUI();
    }
}