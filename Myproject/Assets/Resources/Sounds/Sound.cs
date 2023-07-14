using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Sound", menuName = "Sound")]
public class Sound : ScriptableObject
{
    public AudioClip clip;

    [Range(0, 1)]
    public float volume = 1;

    [Range(0.1f, 3)]
    public float pitch = 1;

    public bool loop;

    [HideInInspector]
    public AudioSource source;

}
