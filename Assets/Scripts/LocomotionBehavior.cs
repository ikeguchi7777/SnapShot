using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocomotionBehavior : StateMachineBehaviour
{
    PlayerInput playerInput;
    bool isAim = false;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        if (playerInput == null)
            playerInput = animator.GetComponent<PlayerInput>();
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        if (!isAim && !animator.IsInTransition(layerIndex))
        {
            playerInput.ChangeFirstPerson(true);
            isAim = true;
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        isAim = false;
    }
}
