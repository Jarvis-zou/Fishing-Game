using UnityEngine;

public class DebugSetup : MonoBehaviour
{
    public DebugPanelController debugPanelController;
    public FishingRodController fishingRodController;
    public FishingManager fishingManager;

    void Start()
    {
        debugPanelController.Register(() => $"Manager State: {fishingManager.GetManagerState()}");
        debugPanelController.Register(() =>
        {
            int[] state = fishingRodController.GetOperationState();
            int operation = state[0];
            int line = state[1];

            string OpState = "Idle";
            if (operation == 0) OpState = "Left";
            else if (operation == 1) OpState = "Right";

            string LineState = "Idle";
            if (line == 0) LineState = "Tighten";
            else if (line == 1) LineState = "Release";

            return $"OpState: {OpState}\nLineState: {LineState}";
        });

        debugPanelController.Register(() =>
        {
            if(fishingManager.GetCurrentFish() != null)
            {
                string info = "";

                float end = fishingManager.GetEndurance();
                info += $"Rod Endurance: {end:F2}\n";

                int[] fishState = fishingManager.GetFishState();
                int direction = fishState[0];
                int isFighting = fishState[1];
                string FishDirection = "Idle";
                if (direction == 0) FishDirection = "Left";
                else if (direction == 1) FishDirection = "Right";

                string Fighting = "Not Fighting";
                if (isFighting == 1) Fighting = "Fighting";

                info += $"FishDirection: {FishDirection}\nFighting: {Fighting}\n";
             
                float fishtStamina = fishingManager.GetFishStamina();

                info += $"Fish Stamina: {fishtStamina:F2}";

                return info;
            }
            return "No Fish";
        });
    }
}
