using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioMixerGroup musicAudioGroup;
    [SerializeField] private AudioMixerGroup sfxAudioGroup;
    private AudioSource musicAudioSource;
    [SerializeField] private Sound defaultMusic;
    private AudioSource sfxAudioSource;
    [SerializeField] private List<Sound> sounds;
    [SerializeField] private Sound defaultSFX;


    void Awake(){
        if (instance == null){
            instance = this;
        } else {
            Destroy(gameObject);
        }

        musicAudioSource = gameObject.AddComponent<AudioSource>();
        musicAudioSource.outputAudioMixerGroup = musicAudioGroup;

        sfxAudioSource = gameObject.AddComponent<AudioSource>();
        sfxAudioSource.outputAudioMixerGroup = sfxAudioGroup;
    }

    void Start(){
        PlayMusic(defaultMusic);
    }

    public void PlayMusic(Sound newMusic){
        if (newMusic != null){
            musicAudioSource.volume = newMusic.volume;
            musicAudioSource.clip = newMusic.clip;
            musicAudioSource.loop = newMusic.loop;

            musicAudioSource.Play();
        }
    }

    public void PlayGlobalSound(string clipName){
        Sound s = sounds.Find(x => x.soundName == clipName);
        
        if (!s){
            s = defaultSFX;
        }

        sfxAudioSource.PlayOneShot(s.clip, s.volume);
    }

    #region VolumeSetters
        public void SetMasterVolume(float value){
            float volume = VolumeConverter(value);
            audioMixer.SetFloat("Volume_Master", volume);
        }

        public void SetMusicVolume(float value){
            float volume = VolumeConverter(value);
            audioMixer.SetFloat("Volume_Music", volume);
        }

        public void SetSFXVolume(float value){
            float volume = VolumeConverter(value);
            audioMixer.SetFloat("Volume_SFX", volume);
        }
    #endregion

    private float VolumeConverter(float valueToConvert){
        Mathf.Clamp(valueToConvert, 0.0001f, 1f);
        return Mathf.Log(valueToConvert) * 20;
    }
}
