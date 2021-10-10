using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoSingleton<SoundManager>
{
    public AudioMixer audioMixer;
    public AudioSource bgSource;
    public AudioClip[] bgclip;



    private void Awake()
    {
        DontDestroyOnLoad(Instance);
    }

    public void SFXPlay(string sfxName, AudioClip clip)
    {
        GameObject soundobj = new GameObject(sfxName + "Sound");
        AudioSource audioSource = soundobj.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("SFX")[0];
        audioSource.clip = clip;
        audioSource.Play();
    }

    public void BgSoundPlay(AudioClip clip)
    {
        bgSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("BackGround")[0];
        bgSource.clip = clip;
        bgSource.loop = true;
        bgSource.Play();
    }

    public void SetBgSoundVolume(int val)
    {
        audioMixer.SetFloat("BackGround", ((val * 10) -80));
        SaveManager.Instance.SaveKeySetting();
    }
    public void SetEffectSoundVolume(int val)
    {
        audioMixer.SetFloat("SFX", ((val * 10) - 80));
        SaveManager.Instance.SaveKeySetting();
    }
}
