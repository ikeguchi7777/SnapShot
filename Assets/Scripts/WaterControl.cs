using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterControl : MonoBehaviour
{
    [SerializeField] RippleControl[] rippleControls;
    List<SnapShotPlayerController> players = new List<SnapShotPlayerController>();
    private void OnTriggerEnter(Collider other)
    {
        var player = other.gameObject.GetComponent<SnapShotPlayerController>();
        if (!players.Contains(player))
        {
            Debug.Log("water in");
            rippleControls[player.PlayerID].enabled = true;
            rippleControls[player.PlayerID].parentObj = player.transform;
            player.ChangeWaterState(true);
        }
        players.Add(player);
    }

    private void OnTriggerExit(Collider other)
    {
        var player = other.gameObject.GetComponent<SnapShotPlayerController>();
        players.Remove(player);
        if (!players.Contains(player))
        {
            Debug.Log("water out");
            rippleControls[player.PlayerID].enabled = false;
            player.ChangeWaterState(false);
        }
    }
}
