using UnityEngine;

public class RodBendController : MonoBehaviour
{
    public Animator animator;
    public string animationStateName = "Take 001";
    private bool isHooked = false;
    private bool isFighting = false;

    private float animationProgress = 0f;
    private float animationLength = 1f;

    [SerializeField]
    private float transitionSpeed = 2f;

    void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
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
        float targetProgress = 0f;

        if (isHooked)
        {
            targetProgress = isFighting ? 1f : 0.3f;
        }
        else
        {
            targetProgress = 0f;
        }

        float delta = (Time.deltaTime / animationLength) * transitionSpeed;
        animationProgress = Mathf.MoveTowards(animationProgress, targetProgress, delta);

        animator.Play(animationStateName, 0, animationProgress);
        animator.speed = 0;
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
