using UnityEngine;
using System.Collections;

public class GenerateFish : MonoBehaviour
{

    [SerializeField] private GameObject fishPrefab;
    private GameObject currentFish;

    private void Update()
    {
        if (currentFish == null)
        {
            StartCoroutine(SpawnFishAfterDelay());
        }
    }

    private IEnumerator SpawnFishAfterDelay()
    {
        // Avoid starting multiple coroutines
        if (isSpawning) yield break;

        isSpawning = true;
        yield return new WaitForSeconds(1f);

        currentFish = Instantiate(fishPrefab);
        currentFish.transform.position = Vector3.one;
        FishController fishController = currentFish.GetComponent<FishController>();
        if (fishController != null)
        {
            fishController.OnHooked();
        }

        isSpawning = false;
    }

    private bool isSpawning = false;
}
