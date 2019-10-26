using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ItemBase : MonoBehaviour
{
    [SerializeField] private float height = 1.0f;
    [SerializeField] protected float EffectDuration = 10.0f;
    [SerializeField] Item type;
    protected MainGameManager manager;
    float lifetime = 30.0f;

    private void Start()
    {
        enabled = false;
        manager = FindObjectOfType<MainGameManager>();
        Destroy(gameObject, lifetime);
    }
    public virtual void Affect(SnapShotPlayerController controller)
    {
        manager.Affect(controller.PlayerID, EffectDuration, type);
        Destroy(gameObject);
    }

    public void FallDown(Vector3 pos)
    {
        transform.DOMove(pos + Vector3.up * height, 3.0f);
    }
}
