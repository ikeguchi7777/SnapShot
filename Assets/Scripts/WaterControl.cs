using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterControl : MonoBehaviour
{
    [SerializeField] RippleControl[] rippleControls=null;
    [SerializeField] GameObject splash=default;
    List<SnapShotPlayerController> players = new List<SnapShotPlayerController>();
    [SerializeField] float splashRange = 5.0f;
    private void OnTriggerEnter(Collider other)
    {
        var player = other.gameObject.GetComponent<SnapShotPlayerController>();
        if (!players.Contains(player))
        {
            Debug.Log("water in");
            rippleControls[player.PlayerID].enabled = true;
            rippleControls[player.PlayerID].parentObj = player.transform;
            player.ChangeWaterState(true);
            if(Vector3.Dot(player._rigidbody.velocity,Vector3.down)>splashRange)
            {
                Instantiate(splash, new Vector3(player.transform.position.x, splash.transform.position.y, player.transform.position.z), splash.transform.rotation);
            }
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
            if (Vector3.Dot(player._rigidbody.velocity, Vector3.up) > splashRange)
            {
                Instantiate(splash, new Vector3(player.transform.position.x, splash.transform.position.y, player.transform.position.z), splash.transform.rotation);
            }
        }
    }
}
