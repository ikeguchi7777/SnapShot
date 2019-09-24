using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transparent : ItemBase
{
    public override void Affect(SnapShotPlayerController controller)
    {
        manager.Affect(controller.PlayerID, EffectDuration,Item.Transparency);
        base.Affect(controller);
    }
}
