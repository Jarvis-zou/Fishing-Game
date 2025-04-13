using UnityEngine;

public class FishController : MonoBehaviour, IFishable
{
    private float stamina = 0;
    private bool hooked = false;

    private enum Direction { Left, Right, Stay }

    private Direction currentDirection;
    private bool isFighting;

    private float directionTimer = 0f;
    private float directionDuration = 2f;

    public void OnCaught()
    {
        isFighting = true;
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


    void Start()
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
        int dir = Random.Range(0, 3); // 0 = Left, 1 = Right, 2 = Stay
        currentDirection = (Direction)dir;
    }

    void GenerateFighting() { 
        int v = Random.Range(1, 11);
        if (v <= 3) isFighting = true;
        isFighting = false;
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

}
