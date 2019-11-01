using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ItemUI : MonoBehaviour
{
    public float duration = 0.5f;
    RectTransform rectTransform;
    ItemUI next = null;
    [SerializeField] Sprite[] sprites=null;

    private void Entry()
    {
        rectTransform.DOAnchorPosX(-20.0f, duration);
    }

    public ItemUI Exit(ItemUI first)
    {
        rectTransform.DOAnchorPosX(20.0f, duration);
        Destroy(gameObject, duration+0.1f);
        if (next != null)
            next.Up(this);
        if (first != this)
        {
            var t = first;
            while (t.next != this)
            {
                t = t.next;
            }
            t.next = next;
        }
        else
        {
            first = next;
        }
        return first;
    }

    public void SetSprite(Item type)
    {
        var im = GetComponent<UnityEngine.UI.Image>();
        im.sprite = sprites[(int)type];
    }

    private void Up(ItemUI pre)
    {
        if (next != null)
            next.Up(this);
        rectTransform.DOAnchorPosY(pre.rectTransform.anchoredPosition.y, duration);
    }

    public ItemUI Set(ItemUI first)
    {
        if (first == null)
        {
            first = this;
            rectTransform.anchoredPosition = new Vector2(20.0f, -20.0f);
        }
        else
        {
            var t = first.GetLast();
            rectTransform.anchoredPosition = new Vector2(20.0f, t.rectTransform.anchoredPosition.y - 40.0f);
            t.next = this;
        }
        Entry();
        return first;
    }

    public ItemUI GetLast()
    {
        if (next == null)
            return this;
        else
            return next.GetLast();
    }
    private void Awake()
    {
        rectTransform = transform as RectTransform;
        Entry();
    }

}
