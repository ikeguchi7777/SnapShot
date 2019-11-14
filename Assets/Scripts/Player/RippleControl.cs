using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RippleControl : MonoBehaviour
{
    public Transform parentObj;
    [SerializeField] ParticleSystem ripple;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(parentObj.position.x, transform.position.y, parentObj.position.z);
    }

    private void OnEnable()
    {
        ripple.Play();
    }

    private void OnDisable()
    {
        ripple.Stop();
    }
}
