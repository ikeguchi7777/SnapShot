using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Invector.CharacterController;

public class SnapShotPlayerController : vThirdPersonController
{
    #region Character Variables
    [Header("--- SnapShotParam ---")]
    [Tooltip("1人称視点のカメラの位置")]
    public Transform Sight;
    [SerializeField,Tooltip("腰のボーン")]
    Transform Spine;
    [SerializeField, Tooltip("腰を回すスピード")]
    float SpineSpeed;
    [SerializeField, Tooltip("腰の可動域")]
    Vector2 SpineRange;
    #endregion
    Vector3 nextEulerAngle;
    Vector3 eulerAngle;
    Vector3 eulerVelocity;
    float x, y;

    public int PlayerID { get; set; }

    void Awake()
    {
        nextEulerAngle = Vector3.zero;
        eulerVelocity = Vector3.zero;
    }

    public void AimCamera(bool value)
    {
        isStrafing = value;
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
        }
        else
            nextEulerAngle = Vector3.zero;
    }

    Vector3 ClampSpineEulerAngle(Vector3 euler)
    {
        return new Vector3(Mathf.Clamp(euler.x, -SpineRange.y, SpineRange.y), Mathf.Clamp(euler.y, -SpineRange.x, SpineRange.x), euler.z);
    }

}
