using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Sound", menuName = "Scriptable Objects/Audio/Sound")]
public class Sound : ScriptableObject
{
    public string soundName = "New Sound";
    [Range(0.0f, 1.0f)] public float volume = 1f;
    public AudioClip clip;
    public bool loop = false;
}
