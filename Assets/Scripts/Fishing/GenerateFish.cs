using UnityEngine;
using System.Collections;

public class GenerateFish : MonoBehaviour
{

    [SerializeField] private GameObject fishPrefab;
    private GameObject currentFish;

    //private void Update()
    //{
    //    if (currentFish == null)
    //    {
    //        StartCoroutine(SpawnFishAfterDelay());
    //    }
    //}

    private IEnumerator SpawnFishAfterDelay(Vector3 Position)
    {
        // Avoid starting multiple coroutines
        if (isSpawning) yield break;

        isSpawning = true;
        yield return new WaitForSeconds(2f);

        currentFish = Instantiate(fishPrefab);
        currentFish.transform.position = new Vector3(Position.x, Position.y + 1, Position.z);
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
        Debug.Log(other.gameObject.name);
        if (other.gameObject.CompareTag("Hook"))
        {
            if (currentFish == null)
            {
                StartCoroutine(SpawnFishAfterDelay(other.gameObject.transform.position));
            }
        }
    }
}
