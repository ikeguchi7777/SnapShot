using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseSystem : MonoBehaviour
{
    class RigidbodyVelocity
    {
        public Vector3 velocity;
        public Vector3 angularVeloccity;
        public RigidbodyVelocity(Rigidbody rigidbody)
        {
            velocity = rigidbody.velocity;
            angularVeloccity = rigidbody.angularVelocity;
        }
    }

    public GameObject[] ignoreGameObjects;

    RigidbodyVelocity[] rigidbodyVelocities;

    Rigidbody[] pausingRigidbodies;

    MonoBehaviour[] pausingMonoBehaviours;

    Animator[] pausingAnimators;

    bool isPauseable = false;
    bool isResumeable = false;
    // Start is called before the first frame update
    void Start()
    {
#if UNITY_EDITOR
        isPauseable = true;
#endif
        enabled = false;
    }

    public void Pause(int id)
    {
        if (!isPauseable)
            return;
        isPauseable = false;
        Predicate<Rigidbody> rgPredicate =
            obj => !obj.IsSleeping() && Array.FindIndex(ignoreGameObjects, igobj => igobj.gameObject == obj.gameObject) < 0;
        pausingRigidbodies = Array.FindAll(GetComponentsInChildren<Rigidbody>(), rgPredicate);
        rigidbodyVelocities = new RigidbodyVelocity[pausingRigidbodies.Length];
        for (int i = 0; i < pausingRigidbodies.Length; i++)
        {
            rigidbodyVelocities[i] = new RigidbodyVelocity(pausingRigidbodies[i]);
            pausingRigidbodies[i].Sleep();
        }
        Predicate<MonoBehaviour> monoPredicate =
            obj => obj.enabled &&
                   obj != this &&
                   Array.FindIndex(ignoreGameObjects, igobj => igobj == obj.gameObject) < 0;
        pausingMonoBehaviours = Array.FindAll(GetComponentsInChildren<MonoBehaviour>(), monoPredicate);
        foreach (var monoBehaviour in pausingMonoBehaviours)
        {
            monoBehaviour.enabled = false;
        }
        Predicate<Animator> aniPredicate =
            obj => obj.isActiveAndEnabled &&
            Array.FindIndex(ignoreGameObjects, igobj => igobj == obj.gameObject) < 0;
        pausingAnimators = Array.FindAll(GetComponentsInChildren<Animator>(), aniPredicate);
        foreach(var animator in pausingAnimators)
        {
            animator.speed = 0.0f;
        }
        isResumeable = true;
    }

    public void Resume()
    {
        if (!isResumeable)
            return;
        isResumeable = false;
        for (int i = 0; i < pausingRigidbodies.Length; i++)
        {
            pausingRigidbodies[i].WakeUp();
            pausingRigidbodies[i].velocity = rigidbodyVelocities[i].velocity;
            pausingRigidbodies[i].angularVelocity = rigidbodyVelocities[i].angularVeloccity;
        }
        foreach(var monoBehaviour in pausingMonoBehaviours)
        {
            monoBehaviour.enabled = true;
        }
        foreach(var animator in pausingAnimators)
        {
            animator.speed = 1.0f;
        }
        isPauseable = true;
    }
}
