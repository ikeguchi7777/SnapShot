using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Invector.CharacterController;
using UnityEngine.UI;

public class TPCamera : vThirdPersonCamera
{
    private int id;

    private Transform SightPosition;
    private RawImage Screen;
    private Slider batbar;
    private GameManager gameManager;
    private RectTransform canvas;
    private ItemUI first = null;
    [SerializeField] RectTransform[] pointers=null;
    [SerializeField] Image background=default;
    [SerializeField] ItemUI itembase=default;

    protected bool isFirstPerson = false;
    public void SetId(int id)
    {
        this.id = id;
        float x = 0.0f, y = 0.0f, w = 0.5f, h = 0.5f;
        if (id < 2 && (GameInstance.Instance.PlayerNum > 2 || id == 0))
            y = 0.5f;
        if (id % 2 == 1 && GameInstance.Instance.PlayerNum > 2)
            x = 0.5f;
        if (GameInstance.Instance.PlayerNum == 2)
            w = 1.0f;
        var viewport = new Rect(x, y, w, h);
        if (!_camera)
            _camera = GetComponent<Camera>();
        foreach (var cam in GetComponentsInChildren<Camera>())
        {
            cam.rect = viewport;
        }
        gameManager = FindObjectOfType<GameManager>();
        canvas = GetComponentInChildren<Canvas>().transform as RectTransform;
    }

    public void SetFirstPerson(bool value)
    {
        isFirstPerson = value;
        Screen.enabled = value;
        background.enabled = value;
    }

    public void SetRenderTexture(RenderTexture texture)
    {
        Screen = GetComponentInChildren<RawImage>();
        Screen.texture = texture;
        Screen.enabled = false;
        background.enabled = false;
    }

    public void SetBatteryBar(SmartPhoneCamera smartPhone)
    {
        batbar = GetComponentInChildren<Slider>();
        smartPhone.OnBatterChanged += value =>
        {
            batbar.value = value;
        };
    }

    protected override void FixedUpdate()
    {
        if (target == null || targetLookAt == null) return;
        if (!isFirstPerson)
            CameraMovement();
    }

    public void ChangeBatteryUI(bool IO)
    {
        if (IO)
        {
            Screen.color = Color.white;
        }
        else
        {
            Screen.color = Color.black;
        }
    }

    public void SetLayer(int layerMask,bool isSet)
    {
        if (isSet)
        {
            _camera.cullingMask |= layerMask;
        }
        else
        {
            _camera.cullingMask &= ~layerMask;
        }
    }

    public Camera GetCamera()
    {
        return _camera;
    }

    public void SetPlayerIcon(Vector3 view_pos,int playerID)
    {
        pointers[playerID].anchoredPosition = new Vector2(view_pos.x *canvas.rect.width , view_pos.y * canvas.rect.height);
        
    }

    public void ClearPlayerIcon()
    {
        foreach (var item in pointers)
        {
            item.anchoredPosition = new Vector2(-100, -100);
        }
    }

    public ItemUI SetItemIcon(Item item)
    {
        var t = Instantiate(itembase, canvas);
        first = t.Set(first);
        t.SetSprite(item);
        if (item == Item.Warp)
            StartCoroutine(RemoveIconQuick(t));
        return t;
    }

    public void RemoveIcon(ItemUI item)
    {
        first = item.Exit(first);
    }
    IEnumerator RemoveIconQuick(ItemUI item)
    {
        yield return new WaitForSeconds(item.duration);
        RemoveIcon(item);
    }
}
