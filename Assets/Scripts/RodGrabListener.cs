using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

public class RodGrabListener : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;

    private bool isGrabbed = false;

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();

        // Subscribe to grab events
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        GameObject gameObject = args.interactableObject.transform.gameObject;
        if (gameObject.CompareTag("FishingRod"))
        {
            StartCoroutine(DelaySetGrabbedTrue());
        }
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        isGrabbed = false;
    }

    private void OnDestroy()
    {
        // Always clean up listeners to avoid memory leaks
        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectExited.RemoveListener(OnRelease);
    }

    public bool GetIsGrabbed()
    {
        return isGrabbed;
    }

    private IEnumerator DelaySetGrabbedTrue()
    {
        yield return new WaitForSeconds(0.1f); // More conservative

        isGrabbed = true;
    }
}
