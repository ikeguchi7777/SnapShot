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
    Camera smCamera;
    [SerializeField] RectTransform[] pointers=null;
    [SerializeField] Image[] backgrounds=null;
    [SerializeField] ItemUI itembase=default;
    [SerializeField] Sprite[] Battery = null;
    [SerializeField] Image BatteryBackground;

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
        BatteryBackground.sprite = Battery[id];
    }

    public void SetFirstPerson(bool value)
    {
        isFirstPerson = value;
        Screen.enabled = value;
        foreach (var item in backgrounds)
        {
            item.enabled = value;
        }
    }

    public void SetRenderTexture(RenderTexture texture)
    {
        Screen = GetComponentInChildren<RawImage>();
        Screen.texture = texture;
        Screen.enabled = false;
        foreach (var item in backgrounds)
        {
            item.enabled = false;
        }
    }

    public void SetPhoneCamera(Camera cam)
    {
        smCamera = cam;
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
            backgrounds[2].gameObject.SetActive(false);
        }
        else
        {
            Screen.color = Color.grey;
            backgrounds[2].gameObject.SetActive(true);
        }
    }

    public void SetLayer(int layerMask,bool isSet)
    {
        if (isSet)
        {
            smCamera.cullingMask |= layerMask;
            _camera.cullingMask |= layerMask;
        }
        else
        {
            smCamera.cullingMask &= ~layerMask;
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
