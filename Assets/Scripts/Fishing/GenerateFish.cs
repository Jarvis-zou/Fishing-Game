using UnityEngine;
using System.Collections;

public class GenerateFish : MonoBehaviour
{

    [SerializeField] private GameObject fishPrefab;
    [SerializeField] private GameObject fishPrefab2;
    private GameObject currentFish;

    //private void Update()
    //{
    //    if (currentFish == null)
    //    {
    //        StartCoroutine(SpawnFishAfterDelay());
    //    }
    //}

    private IEnumerator SpawnFishAfterDelay(Vector3 Position, Transform parent)
    {
        // Avoid starting multiple coroutines
        if (isSpawning) yield break;

        isSpawning = true;
        yield return new WaitForSeconds(2f);


        int option = Random.Range(0, 2);
        if (fishPrefab == null && fishPrefab2 == null) yield break;

        if (option == 0 && fishPrefab != null) currentFish = Instantiate(fishPrefab);
        else currentFish = Instantiate(fishPrefab2);

        currentFish.transform.position = new Vector3(Position.x, Position.y - 0.4f, Position.z);
        currentFish.transform.SetParent(parent);
        FishController fishController = currentFish.GetComponent<FishController>();
        if (fishController != null)
        {
            fishController.OnHooked();
        }

        isSpawning = false;
    }

    private bool isSpawning = false;

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject.name);
        if (other.gameObject.CompareTag("Hook"))
        {
            if (currentFish == null)
            {
                FishingRodController rodController = FishingManager.Instance.GetFishingRodController();
                if (rodController == null) return;
                HookThrower hookThrower = rodController.hookThrower;
                if (hookThrower == null) return;
                GameObject hook = hookThrower.GetCurrentHook();
                if (hook == null) return;
                HookController hookController = hook.GetComponent<HookController>();
                if (hookController == null) return;
                if (hookController.HasFish()) return;
                StartCoroutine(SpawnFishAfterDelay(other.gameObject.transform.position, hook.transform));
            }
        }
    }
}
