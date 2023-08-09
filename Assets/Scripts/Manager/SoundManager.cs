using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager I = null;

    AudioSource bgm;
    List<AudioSource> sfxPool = new List<AudioSource>();

    [SerializeField] AudioClip[] sfxClips = null;

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

        bgm = temp.GetComponent<AudioSource>();
        bgm.loop = true;

        for (int i = 0; i < _poolSize; i++)
        {
            GameObject sfx = Instantiate(new GameObject());
            sfx.transform.parent = transform;
            sfx.AddComponent<AudioSource>();

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

    #endregion
}
