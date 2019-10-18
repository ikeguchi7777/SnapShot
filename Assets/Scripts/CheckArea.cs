﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckArea : MonoBehaviour
{

   // public Collision collision { get; private set;}
    bool[] checks = new bool[GameInstance.Instance.PlayerNum];
    bool tmp;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < checks.Length; i++)
        {
            checks[i] = false;
        }
    }

    // Update is called once per frame
    void Update()
    {

        tmp = true;
        foreach (var item in checks)
        {
            tmp &= item;
        }

        if (tmp)
        {
            GameObject.Find("TutorialManager").gameObject.GetComponent<TutorialManager>().NextPhase();

            Destroy(this.gameObject);
        }

        for (int i = 0; i < checks.Length; i++)//デバック用
        {

        }

    }


    private void OnCollisionEnter(Collision other)
    {
        checks[other.transform.GetComponent<SnapShotPlayerController>().PlayerID] = true;

 
    }
}