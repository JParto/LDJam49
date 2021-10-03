using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    AudioManager audioManager => AudioManager.instance;

    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject optionsMenu;

    void Awake(){
        if (instance == null){
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    public void PauseGame(){
        if (optionsMenu.activeInHierarchy){
            pauseMenu.SetActive(true);
            optionsMenu.SetActive(false);
        } else {
            if (pauseMenu.activeInHierarchy){
                pauseMenu.SetActive(false);
            } else {
                pauseMenu.SetActive(true);
            }
        }
    }

    #region Buttons

        public void Button_ResumeGame(){
            Debug.Log("Should never be used");
        }

        public void Button_QuitGame(){
            Application.Quit();
        }

    #endregion

    #region Sliders
        public void Slider_MasterVolume(float value){
            if (!audioManager){
                Debug.LogWarning("No Active AudioManager!!!");
            }
            audioManager.SetMasterVolume(value);
        }

        public void Slider_MusicVolume(float value){
            if (!audioManager){
                Debug.LogWarning("No Active AudioManager!!!");
            }
            audioManager.SetMusicVolume(value);
        }

        public void Slider_SFXVolume(float value){
            if (!audioManager){
                Debug.LogWarning("No Active AudioManager!!!");
            }
            audioManager.SetSFXVolume(value);
        }
    #endregion

    bool CheckAudioManager(){
        if (audioManager){
            return true;
        } else {
            return false;
        }
    }
}
