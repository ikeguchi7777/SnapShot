using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : MonoBehaviour
{
    [SerializeField] protected float EffectDuration = 10.0f;
    protected MainGameManager manager;

    private void Start()
    {
        enabled = false;
        manager = FindObjectOfType<MainGameManager>();
    }
    public virtual void Affect(SnapShotPlayerController controller)
    {
        Destroy(gameObject);
    }
}
