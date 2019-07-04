using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Invector.CharacterController;

public class SnapShotPlayerController : vThirdPersonController
{
    #region Character Variables
    [Header("--- SnapShotParam ---")]
    public Transform Sight;
    #endregion
    public int PlayerID { get; set; }

    public void AimCamera(bool value)
    {
        isStrafing = value;
    }
}
