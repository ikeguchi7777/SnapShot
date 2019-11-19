using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public static SoundController Instance { get; private set; }
    public enum Sound
    {
       camera,
       count,
       whistle,

    }

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip[] BGMClips=null, SEClips=null;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource.clip = BGMClips[0];
        PlayBGM();
    }



    public void PlaySE(Sound num)
    {
        audioSource.PlayOneShot(SEClips[(int)num]);
    }

    public void PlayBGM()
    {
        audioSource.Play();
    }
}
