using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Invector.CharacterController;

public class PlayerInput : MonoBehaviour
{
    KeyNameList keyname;
    private SnapShotPlayerController cc;

    protected TPCamera tpCamera;

    protected virtual void Start()
    {
        CharacterInit();
    }
    protected virtual void CharacterInit()
    {
        cc = GetComponent<SnapShotPlayerController>();
        if (cc != null)
            cc.Init();
        else
            Debug.LogError("SnapShotPlayerControllerの取得失敗");

        tpCamera = FindObjectOfType<GameManager>().tpCameras[cc.PlayerID];
#if UNITY_EDITOR
        keyname = new KeyNameList(cc.PlayerID);
#else
        keyname = new KeyNameList(cc.PlayerID);
#endif
        if (tpCamera) tpCamera.SetMainTarget(this.transform);
        else Debug.LogError("TPCameraの取得失敗");
        tpCamera.SetRenderTexture(cc.PanelTexture());
        cc.smartPhone.changeBatteryUI = tpCamera.ChangeBatteryUI;
        tpCamera.SetBatteryBar(cc.smartPhone);
        cc.GetComponent<ChangeLayer>().ChangeObjectLayer(cc.PlayerID);
    }

    protected virtual void LateUpdate()
    {
        InputHandle();
        UpdateCameraStates();
    }

    protected virtual void FixedUpdate()
    {
        cc.AirControl();
        CameraInput();
    }

    protected virtual void Update()
    {
        cc.UpdateMotor();
        cc.UpdateAnimator();
    }

    protected virtual void InputHandle()
    {
#if UNITY_EDITOR
        ExitGameInput();
#endif
        CameraInput();

        if (!cc.lockMovement)
        {
            AimCameraInput();
            MoveCharacter();
            if (!cc.isStrafing)
            {
                SprintInput();
                JumpInput();
            }
            else
            {
                SnapInput();
            }
        }
    }

    protected virtual void MoveCharacter()
    {
        cc.input.x = Input.GetAxis(keyname.LHorizontal);
        cc.input.y = Input.GetAxis(keyname.LVertical);
    }

    protected virtual void SprintInput()
    {
        if (Input.GetButtonDown(keyname.Sprint))
            cc.Sprint(true);
        else if (Input.GetButtonUp(keyname.Sprint))
            cc.Sprint(false);
    }

    protected virtual void JumpInput()
    {
        if (Input.GetButtonDown(keyname.Jump))
            cc.Jump();
    }

    protected virtual void AimCameraInput()
    {
        if (Input.GetButtonDown(keyname.AimCamera))
        {
            cc.AimCamera(true);
            tpCamera.SetFirstPerson(true);
        }
        else if (Input.GetButtonUp(keyname.AimCamera))
        {
            cc.AimCamera(false);
            tpCamera.SetFirstPerson(false);
        }
    }

    protected virtual void SnapInput()
    {
        if (Input.GetButtonDown(keyname.Snap))
        {
            cc.TakePhoto();
        }
    }
#if UNITY_EDITOR
    protected virtual void ExitGameInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!Cursor.visible)
                Cursor.visible = true;
            else
                Application.Quit();
        }
    }
#endif

    protected virtual void CameraInput()
    {
        var X = Input.GetAxis(keyname.RHorizontal);
        var Y = Input.GetAxis(keyname.RVertical);

        if (cc.isStrafing)
        {
            cc.RotateSpine(X, Y);
        }
        else
        {
            tpCamera.RotateCamera(X, Y);

            cc.UpdateTargetDirection(tpCamera.transform);

            RotateWithCamera(tpCamera.transform);
        }
    }

    protected virtual void UpdateCameraStates()
    {

    }

    protected virtual void RotateWithCamera(Transform cameraTransform)
    {

    }

    void OnEnable()
    {
        if (keyname == null)
            return;
        if (!Input.GetButton(keyname.AimCamera))
        {
            cc.AimCamera(false);
            tpCamera.SetFirstPerson(false);
        }
        if (!Input.GetButton(keyname.Sprint))
        {
            cc.Sprint(false);
        }
    }
}

public class KeyNameList
{
    public KeyNameList(int PlayerID)
    {
        var t = PlayerID + 1;
        LHorizontal = "LHorizontal" + t;
        LVertical = "LVertical" + t;
        RHorizontal = "RHorizontal" + t;
        RVertical = "RVertical" + t;
        Jump = "Jump" + t;
        Sprint = "Sprint" + t;
        Snap = "Snap" + t;
        AimCamera = "AimCamera" + t;
        Submit = "Submit" + t;
        Cancel = "Cancel" + t;
    }
    public readonly string LHorizontal, LVertical, RHorizontal, RVertical, Jump, Sprint,
                 Snap, AimCamera, Submit, Cancel;
}