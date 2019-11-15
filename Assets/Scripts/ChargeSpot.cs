using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeSpot : MonoBehaviour
{
    [SerializeField] float ChargeRate = 5.0f;
    int num = 0;
    AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (num == 0)
            audioSource.Play();
        other.gameObject.GetComponent<SnapShotPlayerController>().SetCharge(ChargeRate);
        num++;
    }

    private void OnTriggerExit(Collider other)
    {
        other.gameObject.GetComponent<SnapShotPlayerController>().isCharging = false;
        num--;
        if (num == 0)
            audioSource.Stop();
    }
}
