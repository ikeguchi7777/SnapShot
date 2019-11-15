using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByTime : MonoBehaviour
{
    [SerializeField] float time = 2.0f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, time);
    }
}
