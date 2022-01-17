using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    public Animator animator;

    public void KnockDown()
    {
        animator.SetTrigger(AnimationTags.KNOCK_DOWN_TRIGGER);
    }

    public void StandUp()
    {
        animator.SetTrigger(AnimationTags.STAND_UP_TRIGGER);
    }

    public void Hit()
    {
        animator.SetTrigger(AnimationTags.HIT_TRIGGER);
    }

    public void Death()
    {
        animator.SetTrigger(AnimationTags.DEATH_TRIGGER);
    }

    public void ThrowFly()
    {
        animator.SetTrigger(AnimationTags.THROW_FLY_TRIGGER);
    }

    public void Lift()
    {
        animator.SetTrigger(AnimationTags.LIFT_TRIGGER);
    }

    public void LiftOFF()
    {
        animator.SetBool("InLift",false);
    }

    public void LiftON()
    {
        animator.SetBool("InLift", true);
    }
}
