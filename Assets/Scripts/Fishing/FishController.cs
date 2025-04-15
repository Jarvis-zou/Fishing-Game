using UnityEngine;

public class FishController : MonoBehaviour, IFishable
{
    [SerializeField] private GameObject SelfFishPrefab;

    private float stamina;
    private bool hooked = false;

    private enum Direction { Left, Right, Stay }
    private enum FishType { fish1, fish2, fish3 }

    private Direction currentDirection;
    private bool isFighting;

    private float directionTimer = 0f;
    private float directionDuration = 4f;

    public void OnCaught()
    {
        isFighting = false;
        hooked = false;
        FishingManager.Instance.OnCatchSuccess();
        //Debug.Log("Fish caught");
        Destroy(gameObject);
    }

    public void OnEscaped()
    {
        //Debug.Log("Fish escaped");
        Destroy(gameObject);
    }

    public void OnHooked()
    {
        FishingManager.Instance.SetCurrentFish(this);
        hooked = true;
    }


    void Awake()
    {
        stamina = 10.0f;
    }   


    void Update()
    {
        if (hooked)
        {
            if (stamina <= 0)
            {
                OnCaught();
            }
            HandleFighting();
        }
    }

    void HandleFighting()
    {
        directionTimer -= Time.deltaTime;
        if (directionTimer <= 0f)
        {
            GenerateNewDirection();
            directionTimer = directionDuration;
            GenerateFighting();
            //Debug.Log("Fish moves: " + currentDirection);
            //Debug.Log("Fish Fighting: " + isFighting);
        }
        // optionally, check for success/failure conditions
        // e.g., if player fails to respond to direction in time
    }

    void GenerateNewDirection()
    {
        int dir = Random.Range(0, 7); // 0 - 2 = Left, 3 - 5 = Right, 6 = Stay
        if (dir <= 2) currentDirection = currentDirection == Direction.Left ? Direction.Right : Direction.Left;
        else if (dir <= 5) currentDirection = currentDirection == Direction.Right ? Direction.Left : Direction.Right;
        else currentDirection = Direction.Stay;
    }

    void GenerateFighting() { 
        int v = Random.Range(1, 11);
        if (v <= 3) isFighting = true;
        else isFighting = false;
    }

    public bool CheckInput(int direction, bool shrink) { 
        if(currentDirection != (Direction)2) { 
            if(currentDirection != (Direction)direction)
            {
                return false;
            }
            if (shrink && isFighting)
            {
                return false;
            }
        }
        stamina -= Time.deltaTime;
        return true;
    }

    public float GetStamina()
    {
        return stamina;
    }

    public int[] GetFishState()
    {
        return new int[] {(int)currentDirection, isFighting ? 1: 0};
    }

    public int GetFishType()
    {
        int length = System.Enum.GetValues(typeof(FishType)).Length;

        return Random.Range(0, length);
    }

    public GameObject GetCaughtFishPrefab()
    {
        return SelfFishPrefab;
    }
}
