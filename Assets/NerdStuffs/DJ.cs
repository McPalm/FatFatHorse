using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DJ : MonoBehaviour
{

    static public DJ Instance { private set; get; }

    public AudioClip GameMusic;
    public AudioClip MenuMusic;
    public AudioClip CreditSong;

    public void PlayMenuMusic()
    {
        var source = GetComponent<AudioSource>();
        source.loop = false;
        source.clip = MenuMusic;
        source.Play();
    }
    public void Stop()
    {
        GetComponent<AudioSource>().Stop();
    }

    public void PutASockInIt(float duration) => StartCoroutine(Dampen(duration));

    IEnumerator Dampen(float duration)
    {
        var source = GetComponent<AudioSource>();
        source.volume = 0f;
        yield return new WaitForSecondsRealtime(duration);
        for(float f  = 0; f < 1f; f += Time.deltaTime * 3f)
        {
            source.volume = f*f;
            yield return null;
        }
        source.volume = 1f;
    }

    // Start is called before the first frame update
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayGameMusic() => StartCoroutine(GoGoGame());

    IEnumerator GoGoGame()
    {

        var source = GetComponent<AudioSource>();
        source.Stop();
        source.loop = true;
        source.clip = GameMusic;
        yield return new WaitForSeconds(1f);
        source.Play();
    }

    public void PlayCreditSong() => StartCoroutine(CreditRoutine());

    IEnumerator CreditRoutine()
    {
        var source = GetComponent<AudioSource>();
        source.Stop();
        source.loop = false;
        source.clip = CreditSong;
        yield return new WaitForSeconds(1f);
        source.Play();
    }
}
