using UnityEngine;

public class SoundGameManager : MonoBehaviour
{
    [Header("BGM")]
    public AudioSource ambientSource;   // 평상시 배경음
    public AudioSource chaseSource;     // 추격 배경음(별도 AudioSource)

    [Range(0f, 1f)] public float ambientVolume = 0.5f;
    [Range(0f, 1f)] public float chaseVolume = 0.6f;
    public float crossFadeTime = 0.6f;

    private bool isChasePlaying = false;

    void Start()
    {
        // 시작 시 평상시 BGM만
        if (ambientSource != null)
        {
            ambientSource.loop = true;
            ambientSource.volume = ambientVolume;
            ambientSource.Play();
        }

        if (chaseSource != null)
        {
            chaseSource.loop = true;
            chaseSource.volume = 0f;
            chaseSource.Stop();
        }
    }

    public void SetChase(bool chase)
    {
        if (chase == isChasePlaying) return;
        isChasePlaying = chase;

        StopAllCoroutines();
        StartCoroutine(CrossFade(chase));
    }

    System.Collections.IEnumerator CrossFade(bool toChase)
    {
        if (ambientSource == null || chaseSource == null) yield break;

        if (toChase && !chaseSource.isPlaying)
            chaseSource.Play();

        float t = 0f;
        float aStart = ambientSource.volume;
        float cStart = chaseSource.volume;

        float aTarget = toChase ? 0f : ambientVolume;
        float cTarget = toChase ? chaseVolume : 0f;

        while (t < crossFadeTime)
        {
            t += Time.deltaTime;
            float k = Mathf.Clamp01(t / crossFadeTime);

            ambientSource.volume = Mathf.Lerp(aStart, aTarget, k);
            chaseSource.volume = Mathf.Lerp(cStart, cTarget, k);

            yield return null;
        }

        ambientSource.volume = aTarget;
        chaseSource.volume = cTarget;

        if (!toChase) chaseSource.Stop();
    }
}
