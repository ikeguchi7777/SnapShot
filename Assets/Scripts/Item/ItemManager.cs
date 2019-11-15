using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField] NavMeshSamplePosition[] samplePositions=null;
    [SerializeField] ItemBase[] items=null;
    Coroutine coroutine;
    [SerializeField] float interval = 5.0f;
    [SerializeField] float time_range = 1.0f;
    [SerializeField] int dropNum = 4;
    GameManager gameManager;


    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    IEnumerator FallItem()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(interval-time_range, interval+time_range));
            Vector3 pos;
            for (int i = 0; i < dropNum; i++)
            {
                int j = Random.Range(0, samplePositions.Length);
                int id = Random.Range(0, items.Length);
                samplePositions[j].RandomPoint(out pos);
                var item = Instantiate(items[id], pos + Vector3.up * 50.0f, Quaternion.identity, gameManager.transform);
                item.FallDown(pos);
            }
        }
    }

    private void OnEnable()
    {
        coroutine = StartCoroutine(FallItem());
    }

    private void OnDisable()
    {
        StopCoroutine(coroutine);
    }
}
