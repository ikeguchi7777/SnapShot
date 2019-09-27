using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLayer : MonoBehaviour
{
    [SerializeField] GameObject[] gameObjects;

    public void ChangeObjectLayer(int PlayerID)
    {
        foreach (var item in gameObjects)
        {
            item.layer = LayerMask.NameToLayer((PlayerID + 1) + "P");
        }
        Destroy(this);
    }
}
