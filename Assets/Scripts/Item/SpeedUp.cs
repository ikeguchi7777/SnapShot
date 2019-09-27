using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUp : ItemBase
{
    public override void Affect(SnapShotPlayerController controller)
    {
        manager.Affect(controller.PlayerID, EffectDuration, Item.SpeedUp);
        base.Affect(controller);
    }
}
