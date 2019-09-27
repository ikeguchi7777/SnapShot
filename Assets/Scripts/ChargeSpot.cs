using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeSpot : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.GetComponent<SnapShotPlayerController>().isCharging = true;
    }

    private void OnTriggerExit(Collider other)
    {
        other.gameObject.GetComponent<SnapShotPlayerController>().isCharging = false;
    }
}
