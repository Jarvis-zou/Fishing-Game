using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Readers;

public class HoldToPlayAnimation : MonoBehaviour
{
    public Animator animator;
    public string animationStateName = "Take 001"; // Change this to the animation name inside your FBX file
    private float animationProgress = 0f;
    private float animationLength;
    private bool isHolding = false;

    [SerializeField]
    XRInputValueReader<float> m_TriggerInput = new XRInputValueReader<float>("Trigger");

    void Start()
    {
        // Automatically find Animator if not assigned
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        // Find the animation length
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach (var clip in clips)
        {
            if (clip.name == animationStateName)
            {
                animationLength = clip.length;
                break;
            }
        }
    }

    void Update()
    {
        // Get the current trigger value using XRInputValueReader
        float triggerValue = m_TriggerInput.ReadValue();


        // If trigger is pressed, play forward
        isHolding = triggerValue > 0.1f; // Threshold of 0.1f for detecting trigger press

        // Control animation progress
        if (isHolding)
        {
            if (animationProgress < 1f)
            {
                animationProgress += Time.deltaTime / animationLength;
            }
        }
        else
        {
            if (animationProgress > 0f)
            {
                animationProgress -= Time.deltaTime / animationLength;
            }
        }

        // Clamp the value between 0 and 1
        animationProgress = Mathf.Clamp(animationProgress, 0f, 1f);

        // Apply the animation progress manually to the animator
        animator.Play(animationStateName, 0, animationProgress);
        animator.speed = 0; // Disable Unity's automatic playback
    }
}