using UnityEngine;

public class RodBendController : MonoBehaviour
{
    public Animator animator;
    public string animationStateName = "Take 001"; // Change to your actual animation name
    private bool isHooked = false;
    private bool isFighting = false;

    private float animationProgress = 0f;
    private float animationLength = 1f;

    [SerializeField]
    private float transitionSpeed = 2f; // Multiplier to speed up/slow down transition

    void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        // Get the length of the specified animation clip
        var clips = animator.runtimeAnimatorController.animationClips;
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
        // Decide target animation progress based on state
        float targetProgress = 0f;

        if (isHooked)
        {
            targetProgress = isFighting ? 1f : 0.3f;
        }
        else
        {
            targetProgress = 0f;
        }

        // Smoothly move animationProgress toward target
        float delta = (Time.deltaTime / animationLength) * transitionSpeed;
        animationProgress = Mathf.MoveTowards(animationProgress, targetProgress, delta);

        // Apply progress to animator
        animator.Play(animationStateName, 0, animationProgress);
        animator.speed = 0; // Pause automatic playback
    }

    public void SetHooked(bool hooked)
    {
        isHooked = hooked;
        if (!isHooked) { isFighting = false; }
    }

    public void SetIsFighting(bool fighting)
    {
        if (!isHooked) { return; }
        isFighting = fighting;
    }
}
