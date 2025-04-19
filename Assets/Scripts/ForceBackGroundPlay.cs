using UnityEngine;

public class ForceBackgroundPlay: MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip audioClip;

    void Start()
    {

        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.clip = audioClip;
            audioSource.Play();
        }
    }
}
