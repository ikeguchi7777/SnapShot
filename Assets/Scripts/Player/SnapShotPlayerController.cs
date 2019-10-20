using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Invector.CharacterController;
using UniRx.Async;

public class SnapShotPlayerController : vThirdPersonController
{
    #region Character Variables
    [Header("--- SnapShotParam ---")]
    [Tooltip("1人称視点のカメラの位置")]
    public Transform Sight=null;
    [SerializeField,Tooltip("腰のボーン")]
    Transform Spine=null;
    [SerializeField, Tooltip("腰を回すスピード")]
    float SpineSpeed=100.0f;
    [SerializeField, Tooltip("腰の可動域")]
    Vector2 SpineRange = new Vector2(30.0f, 40.0f);
    #endregion
    Vector3 nextEulerAngle;
    Vector3 eulerAngle;
    Vector3 eulerVelocity;
    TPCamera _tpCamera;
    public SmartPhoneCamera smartPhone { get; private set; }
    PlayerBodyPoint point;
    float x, y;
    public bool isCharging { get; set; } 

    public int PlayerID { get; set; }

    public Camera currentCamera { get; private set; }

    void Awake()
    {
        nextEulerAngle = Vector3.zero;
        eulerVelocity = Vector3.zero;
        smartPhone = GetComponentInChildren<SmartPhoneCamera>();
        point = GetComponent<PlayerBodyPoint>();
    }

    public void SetTPCamera(TPCamera cam)
    {
        _tpCamera = cam;
        currentCamera = _tpCamera.GetCamera();
    }

    public void AimCamera(bool value)
    {
        isStrafing = value;
        Sprint(false);
        if (value)
            currentCamera = smartPhone._camera;
        else
            currentCamera = _tpCamera.GetCamera();
    }

    public void RotateSpine(float x,float y)
    {
        this.x = x;
        this.y = y;
        nextEulerAngle = ClampSpineEulerAngle(nextEulerAngle + new Vector3(-y * SpineSpeed * Time.deltaTime, x * SpineSpeed * Time.deltaTime, 0.0f));
        eulerAngle = Vector3.SmoothDamp(eulerAngle, nextEulerAngle, ref eulerVelocity, 0.2f);
    }

    void LateUpdate()
    {
        if (isStrafing)
        {
            Spine.rotation *= Quaternion.Euler(eulerAngle);
            smartPhone.ConsumeStandbyPower(Time.deltaTime);
        }
        else
            nextEulerAngle = Vector3.zero;
        if (isCharging)
        {
            smartPhone.ChargeBattery(5.0f * Time.deltaTime);
        }
    }

    Vector3 ClampSpineEulerAngle(Vector3 euler)
    {
        return new Vector3(Mathf.Clamp(euler.x, -SpineRange.y, SpineRange.y), Mathf.Clamp(euler.y, -SpineRange.x, SpineRange.x), euler.z);
    }

    public RenderTexture PanelTexture()
    {
        return smartPhone._panelTexture;
    }

    public void TakePhoto()
    {
        if (!smartPhone.Useable)
            return;
        smartPhone.TakePhoto(PlayerID);
        SoundController.Instance.PlaySE(SoundController.Sound.camera);
    }

    public int CalculateScore(Camera _camera)
    {
        return point.CalculateScore(_camera);
    }

    public void ChangeSpeed(float rate)
    {
        freeSprintSpeed *= rate;
    }

    public bool IsInViewport(Camera cam)
    {
        foreach (var item in point.everyPoint)
        {
            Vector3 view_pos = cam.WorldToViewportPoint(item.position);
            if (!(view_pos.x < -0.0f ||
               view_pos.x > 1.0f ||
               view_pos.y < -0.0f ||
               view_pos.y > 1.0f) && (Vector3.Dot(cam.transform.forward, item.position - cam.transform.position) > 0))
                return true;
        }
        return false;
    }

    public bool IsInViewport(Camera cam,out Vector3 view_pos)
    {
        view_pos = cam.WorldToViewportPoint(point.everyPoint[0].position);
        if (!(view_pos.x < -0.0f ||
           view_pos.x > 1.0f ||
           view_pos.y < -0.0f ||
           view_pos.y > 1.0f) && (Vector3.Dot(cam.transform.forward, transform.position - cam.transform.position) > 0))
            return true;
        return false;
    }
}
