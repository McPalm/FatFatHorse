using System.Collections;
using UnityEngine;
using ObjectPooling;
using System;

public class AudioPool : ObjectPool
{
    static AudioPool _instance;
    void Start() => _instance = this;

    public bool mute = false;

    static public void PlaySoundBypass(Vector3 position, AudioClip clip, float volume = 1f, float pitch = 1f) => _instance.IPlaySound(position, clip, volume, pitch, 20000, true);
    static public void PlaySound(Vector3 position, AudioClip clip, float volume = 1f, float pitch = 1f) => _instance.IPlaySound(position, clip, volume, pitch, 20000);
    void IPlaySound(Vector3 position, AudioClip clip, float volume, float pitch, int lowpass, bool bypass = false) => StartCoroutine(PlaySoundRoutine(position, clip, volume, pitch, lowpass, bypass));

    IEnumerator PlaySoundRoutine(Vector3 position, AudioClip clip, float volume, float pitch, int lowpassfilter, bool bypass)
    {
        if (!mute && volume > 0f && ActiveObjects < 32 && clip != null)
        {

            var audio = Create().GetComponent<AudioSource>();
            audio.clip = clip;
            audio.Play();
            audio.volume = volume;
            /*var lowpass = audio.GetComponent<AudioLowPassFilter>();
            lowpass.enabled = lowpassfilter < 20000;
            lowpass.cutoffFrequency = lowpassfilter;*/
            while (audio.isPlaying)
            {
                audio.pitch = pitch;
                yield return null;
            }
            Dispose(audio.gameObject);

        }
    }
    internal static void PlaySoundLowpass(Vector3 position, AudioClip clip, float volume, int frequency) => _instance.IPlaySound(position, clip, volume, 1f, frequency);
}

