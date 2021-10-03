using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioPlayer : MonoBehaviour
{
    private AudioSource audioSource;

    [SerializeField] private AudioMixerGroup mixerGroup;

    [SerializeField] private List<Sound> sounds;

    void Awake(){
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = mixerGroup;
    }


    public void PlaySound(string clipName, bool loop = false){
        if (!loop){
            Sound s = sounds.Find(x => x.soundName == clipName);

            audioSource.PlayOneShot(s.clip, s.volume);
        } else {
            Sound s = sounds.Find(x => x.soundName == clipName);

            audioSource.clip = s.clip;
            audioSource.volume = s.volume;
            audioSource.loop = true;

            audioSource.Play();

        }
    }

    public void StopLoopSound(){
        audioSource.Stop();
    }
}
