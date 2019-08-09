using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public static SoundController Instance { get; private set; }
    public enum Sound
    {
       camera,
    }

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip[] BGMClips, SEClips;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //audioSource.PlayOneShot(BGMClips[0]);
        //audioSource.loop = true;
        //audioSource.PlayScheduled(AudioSettings.dspTime + BGMClips[0].length);

    }
    // Update is called once per frame
    void Update()
    {

    }


    public void PlaySE(Sound num)
    {
        audioSource.PlayOneShot(SEClips[(int)num]);
    }

    public void PlayBGM()
    {
        // audioSource.loop = true;
        audioSource.Play();
    }
}
