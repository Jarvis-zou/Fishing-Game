using System.Collections.Generic;
using UnityEngine;

public class WaterTrigger : MonoBehaviour
{
    public GameObject splashEffectPrefab;
    private List<Rigidbody> objectsInWater = new List<Rigidbody>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody != null && !objectsInWater.Contains(other.attachedRigidbody))
        {
            if (splashEffectPrefab != null)
            {
                Vector3 splashPos = other.ClosestPoint(transform.position);
                Instantiate(splashEffectPrefab, splashPos, Quaternion.identity);
            }
            objectsInWater.Add(other.attachedRigidbody);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.attachedRigidbody != null)
        {
            objectsInWater.Remove(other.attachedRigidbody);
        }
    }

    private void FixedUpdate()
    {
        foreach (var rb in objectsInWater)
        {
            if(rb != null) ApplyBuoyancy(rb);
        }
    }

    private void ApplyBuoyancy(Rigidbody rb)
    {
        Bounds waterBounds = GetComponent<Collider>().bounds;

        // Get center of object
        if (rb == null) return;
        Vector3 objPos = rb.worldCenterOfMass;

        // Check if inside the water cube
        if (waterBounds.Contains(objPos))
        {
            float objectHeight = rb.transform.lossyScale.y;
            float objectBottom = objPos.y - (objectHeight / 2f);
            float waterTop = waterBounds.max.y;
            float submergedDepth = Mathf.Clamp(waterTop - objectBottom, 0f, objectHeight);
            float submergedFraction = submergedDepth / objectHeight;

            float volume = rb.transform.lossyScale.x * rb.transform.lossyScale.y * rb.transform.lossyScale.z;
            float forceAmount = 5f * 9.81f * volume * submergedFraction;

            float speed = rb.linearVelocity.magnitude;
            rb.AddForce(Vector3.up * forceAmount - rb.linearVelocity * Dampling(speed), ForceMode.Force);
        }
    }

    private float Dampling(float speed)
    {
        if (speed <= 2.0f) return 0.8f;
        else if (speed >= 5.0f) return 21.8f;
        else return 7.0f * speed - 13.2f;
    }
}
