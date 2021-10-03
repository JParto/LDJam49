using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    AudioSource audioSource;
    [SerializeField] private AudioMixerGroup musicMixerGroup;
    [SerializeField] private Sound menuMusic;


    void Awake(){
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = musicMixerGroup;
        audioSource.clip = menuMusic.clip;
        audioSource.volume = menuMusic.volume;
        audioSource.loop = menuMusic.loop;
    }

    void Start(){
        audioSource.Play();
    }





    #region Buttons

        public void Button_StartGame(){
            SceneManager.LoadScene("MainScene");
            SceneManager.LoadScene("Managers", LoadSceneMode.Additive);
            SceneManager.LoadScene("UI", LoadSceneMode.Additive);
            SceneManager.LoadScene("DarkScene", LoadSceneMode.Additive);
            SceneManager.LoadScene("LightScene", LoadSceneMode.Additive);
            SceneManager.LoadScene("Portals", LoadSceneMode.Additive);
        }

        public void Button_QuitGame(){
            Application.Quit();
        }

        public void Button_RestartGame(){
            SceneManager.LoadScene("MainMenu");
        }

    #endregion

    #region Sliders
        public void Slider_MasterVolume(float value){
            float volume = VolumeConverter(value);
            audioMixer.SetFloat("Volume_Master", volume);
        }

        public void Slider_MusicVolume(float value){
            float volume = VolumeConverter(value);
            audioMixer.SetFloat("Volume_Music", volume);
        }

        public void Slider_SFXVolume(float value){
            float volume = VolumeConverter(value);
            audioMixer.SetFloat("Volume_SFX", volume);
        }
    #endregion

    private float VolumeConverter(float valueToConvert){
        Mathf.Clamp(valueToConvert, 0.0001f, 1f);
        return Mathf.Log(valueToConvert) * 20;
    }
}
