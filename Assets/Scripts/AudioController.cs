using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioClip walkClip;
    public AudioClip runClip;
    public AudioClip chaseClip;

    [Header("Volumes")]
    [Range(0f, 3f)] public float footstepVolume = 1.5f;
    [Range(0f, 3f)] public float chaseVolume = 2.0f;   

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayWalk()
    {
        if (walkClip != null)
            audioSource.PlayOneShot(walkClip, footstepVolume);
    }

    public void PlayRun()
    {
        if (runClip != null)
            audioSource.PlayOneShot(runClip, footstepVolume);
    }

    public void StartChaseSound()
    {
        if (chaseClip == null) return;

        audioSource.clip = chaseClip;
        audioSource.loop = true;
        audioSource.volume = chaseVolume;
        audioSource.Play();
    }

    public void StopChaseSound()
    {
        if (audioSource.loop)
        {
            audioSource.Stop();
            audioSource.loop = false;
            audioSource.clip = null;
            audioSource.volume = 1f;
        }
    }
}