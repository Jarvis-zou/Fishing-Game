using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class AddXRController : MonoBehaviour
{
    void Start()
    {
        gameObject.AddComponent<XRController>();
    }
}
