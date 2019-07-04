using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Invector.CharacterController;

public class SnapShotPlayerController : vThirdPersonController
{
    public int PlayerID{get;set;}
    #region SnapShotParam
    [Header("--- SnapShot Setup ---")]
    public Camera myCamera;
    #endregion
}
