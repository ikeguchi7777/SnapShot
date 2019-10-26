using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindOtherPlayer : ItemBase
{
    public override void Affect(SnapShotPlayerController controller)
    {
        manager.Affect(controller.PlayerID, EffectDuration, Item.FindPlayer);
        base.Affect(controller);
    }
}
