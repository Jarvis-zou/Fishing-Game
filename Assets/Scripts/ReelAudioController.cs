using UnityEngine;

public class ReelAudioController : MonoBehaviour
{
    private enum SpinDirectionState { Clockwise, CounterClockwise, Idle }

    public AudioClip clockwiseClip;
    public AudioClip counterClockwiseClip;

    private AudioSource audioSource;
    private SpinDirectionState currentState = SpinDirectionState.Idle;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
    }

    public void SetSpinState(int state)
    {
        SpinDirectionState newState = (SpinDirectionState)state;
        if (newState == currentState) return;

        currentState = newState;

        switch (currentState)
        {
            case SpinDirectionState.Clockwise:
                PlayLoopingClip(clockwiseClip);
                break;
            case SpinDirectionState.CounterClockwise:
                PlayLoopingClip(counterClockwiseClip);
                break;
            case SpinDirectionState.Idle:
                audioSource.Stop();
                break;
        }
    }

    private void PlayLoopingClip(AudioClip clip)
    {
        if (audioSource.clip == clip && audioSource.isPlaying) return;

        audioSource.clip = clip;
        audioSource.Play();
    }
}