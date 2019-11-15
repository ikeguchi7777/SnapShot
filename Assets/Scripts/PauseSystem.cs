using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseSystem : MonoBehaviour
{

    EventSystem eventSystem;
    StandaloneInputModule inputModule;
    [SerializeField] GameObject panel = default;
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
    public bool isPaused { get; private set; } = false;
    // Start is called before the first frame update
    void Start()
    {
        isPauseable = true;
        enabled = false;
        eventSystem = FindObjectOfType<EventSystem>();
        inputModule = eventSystem.gameObject.GetComponent<StandaloneInputModule>();
    }

    public void Pause(int id)
    {
        if (!isPauseable)
            return;
        eventSystem.SetSelectedGameObject(null);
        panel.SetActive(true);
        isPauseable = false;
        ChangeKey(id);
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
        isPaused = true;
        eventSystem.SetSelectedGameObject(panel.GetComponentInChildren<UnityEngine.UI.Button>().gameObject);
        Debug.Log(eventSystem.currentSelectedGameObject);
    }

    public void Resume()
    {
        if (!isPaused)
            return;
        isPaused = false;
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
        panel.SetActive(false);
    }

    public void BackToTitle()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Title");
    }

    void ChangeKey(int id)
    {
        inputModule.horizontalAxis = "LHorizontal" + id;
        inputModule.verticalAxis = "LVertical" + id;
        inputModule.submitButton = "Submit" + id;
    }
}
