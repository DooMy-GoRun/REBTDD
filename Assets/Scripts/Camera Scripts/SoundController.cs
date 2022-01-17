using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip level1_sound, checkpoint_sound;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource.volume = 0.2f;
        audioSource.clip = level1_sound;
        audioSource.Play();

        audioSource.PlayOneShot(checkpoint_sound);
    }
}
