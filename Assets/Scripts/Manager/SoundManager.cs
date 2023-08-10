using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager I = null;

    AudioSource bgm;
    List<AudioSource> sfxPool = new List<AudioSource>();

    [SerializeField] AudioClip[] bgmClips = null;
    [SerializeField] AudioClip[] sfxClips = null;
    [SerializeField] AudioMixer lowPass = null;

    Coroutine fadeOutBGMcoroutine = null;

    #region Singleton
    private void Awake()
    {
        if (I == null)
        {
            I = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    private void Start()
    {
        Init(20);
    }

    #region Initialize
    public void Init(int _poolSize)
    {
        Clear();

        GameObject temp = Instantiate(new GameObject());
        temp.transform.parent = transform;
        temp.AddComponent<AudioSource>();
        temp.name = "BGM";

        bgm = temp.GetComponent<AudioSource>();
        bgm.loop = true;

        for (int i = 0; i < _poolSize; i++)
        {
            GameObject sfx = Instantiate(new GameObject());
            sfx.transform.parent = transform;
            sfx.AddComponent<AudioSource>();
            sfx.name = "SFXSource_" + i;

            sfxPool.Add(sfx.GetComponent<AudioSource>());
        }
    }

    public void Clear()
    {
        Destroy(bgm);

        for (int i = 0; i < sfxPool.Count; i++)
        {
            Destroy(sfxPool[i]);
        }

        sfxPool.Clear();
    }
    #endregion

    #region Play and stop

    public void PlayBGM(int _index)
    {
        bgm.volume = 1.0f;
        bgm.clip = bgmClips[_index];
        bgm.Play();
    }

    public void PlaySFX(int _index)
    {
        for(int i = 0; i < sfxPool.Count; i++)
        {
            if(sfxPool[i].isPlaying == false)
            {
                sfxPool[i].clip = sfxClips[_index];
                sfxPool[i].Play();

                return;
            }
        }

        sfxPool[0].clip = sfxClips[_index];
        sfxPool[0].Play();
    }

    public void StopBGM()
    {
        bgm.Stop();
    }

    public void LowPassBGM()
    {
        if (bgm.outputAudioMixerGroup == null)
        {
            bgm.outputAudioMixerGroup = lowPass.FindMatchingGroups("Master")[0];
        }
        else
        {
            bgm.outputAudioMixerGroup = null;
        }
    }

    public void LowBassBGMOff()
    {
        bgm.outputAudioMixerGroup = null;
    }

    public void FadeOutBGM()
    {
        if(fadeOutBGMcoroutine != null)
        {
            StopCoroutine(FadeOutBGMIEnum());
            fadeOutBGMcoroutine = null;
        }

        fadeOutBGMcoroutine = StartCoroutine(FadeOutBGMIEnum());
    }

    IEnumerator FadeOutBGMIEnum()
    {
        float timePassed = 0.0f;
        float duration = 3.0f;

        float startValue = bgm.volume;

        while (timePassed <= duration)
        {
            float t = timePassed / duration;

            float calculatedValue = Mathf.Lerp(startValue, 0, t);
            bgm.volume = calculatedValue;

            timePassed += Time.deltaTime;

            yield return null;
        }

        fadeOutBGMcoroutine = null;

        yield break;
    }

    #endregion
}
