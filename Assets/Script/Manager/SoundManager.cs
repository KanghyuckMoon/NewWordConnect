using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class SoundManager : MonoSingleton<SoundManager>
{
    public AudioMixer audioMixer;
    public AudioSource[] audioSource;
    public AudioClip[] bgclip;



    private void Awake()
    {
        DontDestroyOnLoad(Instance);
        audioSource = GetComponents<AudioSource>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        BgSoundPlay(bgclip[arg0.buildIndex]);
    }

    public void SFXPlay(int index)
    {
        //audioSource[index].Play();
    }

    public void BgSoundPlay(AudioClip clip)
    {
        audioSource[0].outputAudioMixerGroup = audioMixer.FindMatchingGroups("BackGround")[0];
        audioSource[0].clip = clip;
        audioSource[0].loop = true;
        audioSource[0].Play();
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
