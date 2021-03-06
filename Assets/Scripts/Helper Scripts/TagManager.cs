using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTags
{
    public const string MOVEMENT = "Movement";

    public const string PUNCH_1_TRIGGER = "Punch1";
    public const string PUNCH_2_TRIGGER = "Punch2";
    public const string PUNCH_3_TRIGGER = "Punch3";

    public const string KICK_1_TRIGGER = "Kick1";
    public const string KICK_2_TRIGGER = "Kick2";

    public const string ATTACK_1_TRIGGER = "Attack1";
    public const string ATTACK_2_TRIGGER = "Attack2";
    public const string ATTACK_3_TRIGGER = "Attack3";
    public const string ATTACK_1_COMBO_TRIGGER = "Attack1Combo";
    public const string ATTACK_1_FINISH_TRIGGER = "Attack1Finish";

    public const string IDLE_ANIMATION = "Idle";

    public const string KNOCK_DOWN_TRIGGER = "KnockDown";
    public const string STAND_UP_TRIGGER = "StandUp";
    public const string HIT_TRIGGER = "Hit";
    public const string DEATH_TRIGGER = "Death";

    public const string LIFT_TRIGGER = "onLift";

    public const string THROW_FLY_TRIGGER = "ThrowFly";
}

public class Axis
{
    public const string HORIZONTAL_AXIS = "Horizontal";
    public const string VERTICAL_AXIS = "Vertical";
}

public class Tags
{
    public const string GROUND_TAG = "Ground";
    public const string PLAYER_TAG = "Player";
    public const string ENEMY_TAG = "Enemy";
    public const string STUN_TAG = "Stun";

    
    public const string SUPER_HAND_TAG = "SuperHand";
    public const string SUPER_LEG_TAG = "SuperLeg";
    public const string SUPER_HEAD_TAG = "SuperHead";

    public const string FINISH1_HAND_TAG = "Finish1Hand";
    public const string FINISH_HAND_TAG = "FinishHand";
    public const string FINISH_LEG_TAG = "FinishLeg";
    public const string HEAD_TAG = "Head";
    public const string COMBO_HAND_TAG = "ComboHand";

    public const string THROW_HAND_TAG = "ThrowHand";

    public const string ZERO_HAND_TAG = "ZeroHand";

    //public const string LEFT_HAND_TAG = "LeftHand";
    //public const string LEFT_LEG_TAG = "LeftLeg";
    //public const string RIGHT_HAND_TAG = "RightHand";
    //public const string RIGHT_LEG_TAG = "RightLeg";


    

    public const string UNTAGGED_TAG = "Untagged";
    public const string MAIN_CAMERA_TAG = "MainCamera";
    public const string HEALTH_UI = "HealthUI";
}
