using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationDelegate : MonoBehaviour
{
    public GameObject left_Hand_Attack_Point, right_Hand_Attack_Point, left_Leg_Attack_Point, right_Leg_Attack_Point, head_Attack_Point, character;

    private float stand_Up_Timer = 0.6f;

    private CharacterAnimation animationScript;

    private void Awake()
    {
        animationScript = GetComponent<CharacterAnimation>();
    }

    void Left_Hand_Attack_On()
    {
        left_Hand_Attack_Point.SetActive(true);
    }

    void Left_Hand_Attack_Off()
    {
        if(left_Hand_Attack_Point.activeInHierarchy)
            left_Hand_Attack_Point.SetActive(false);
    }

    void Right_Hand_Attack_On()
    {
        right_Hand_Attack_Point.SetActive(true);
    }

    void Right_Hand_Attack_Off()
    {
        if(right_Hand_Attack_Point.activeInHierarchy)
            right_Hand_Attack_Point.SetActive(false);
    }

    void Head_Attack_On()
    {
        head_Attack_Point.SetActive(true);
    }

    void Head_Attack_Off()
    {
        if(head_Attack_Point.activeInHierarchy)
            head_Attack_Point.SetActive(false);
    }

    void Left_Leg_Attack_On()
    {
        left_Leg_Attack_Point.SetActive(true);
    }

    void Left_Leg_Attack_Off()
    {
        if(left_Leg_Attack_Point.activeInHierarchy)
            left_Leg_Attack_Point.SetActive(false);
    }

    void Right_Leg_Attack_On()
    {
        right_Leg_Attack_Point.SetActive(true);
    }

    void Right_Leg_Attack_Off()
    {
        if(right_Leg_Attack_Point.activeInHierarchy)
            right_Leg_Attack_Point.SetActive(false);
    }


    //Tags for deal damage in animations on the opponents

    void TagSuper_Hand()
    {
        right_Hand_Attack_Point.tag = Tags.SUPER_HAND_TAG;
    }
    void UnTagSuper_Hand()
    {
        right_Hand_Attack_Point.tag = Tags.UNTAGGED_TAG;
    }
    void TagFinish_Hand()
    {
       right_Hand_Attack_Point.tag = Tags.FINISH_HAND_TAG;
    }
    void UnTagFinish_Hand()
    {
        right_Hand_Attack_Point.tag = Tags.UNTAGGED_TAG;
    }
    void TagCombo_Hand()
    {
        left_Hand_Attack_Point.tag = Tags.COMBO_HAND_TAG;
    }
    void UnTagCombo_Hand()
    {
        left_Hand_Attack_Point.tag = Tags.UNTAGGED_TAG;
    }
    void TagSuper_Leg()
    {
        right_Leg_Attack_Point.tag = Tags.SUPER_LEG_TAG;
    }
    void UnTagSuper_Leg()
    {
        right_Leg_Attack_Point.tag = Tags.UNTAGGED_TAG;
    }
    void TagSuper_Head()
    {
        head_Attack_Point.tag = Tags.SUPER_HEAD_TAG;
    }
    void UnTagSuper_Head()
    {
        head_Attack_Point.tag = Tags.UNTAGGED_TAG;
    }
    void TagHead()
    {
        head_Attack_Point.tag = Tags.HEAD_TAG;
    }

    void UnTagHead()
    {
        head_Attack_Point.tag = Tags.UNTAGGED_TAG;
    }

    void TagThrow()
    {
        right_Hand_Attack_Point.tag = Tags.THROW_HAND_TAG;
    }

    void UnTagThrow()
    {
        right_Hand_Attack_Point.tag = Tags.UNTAGGED_TAG;
    }
    //void TagRight_Hand()
    //{
    //    right_Hand_Attack_Point.tag = Tags.RIGHT_HAND_TAG;
    //}

    //void UnTagRight_Hand()
    //{
    //    right_Hand_Attack_Point.tag = Tags.UNTAGGED_TAG;
    //}

    //void TagRight_Leg()
    //{
    //    right_Leg_Attack_Point.tag = Tags.RIGHT_LEG_TAG;
    //}

    //void UnTagRight_Leg()
    //{
    //    right_Leg_Attack_Point.tag = Tags.UNTAGGED_TAG;
    //}

    //void TagLeft_Hand()
    //{
    //    left_Hand_Attack_Point.tag = Tags.LEFT_HAND_TAG;
    //}

    //void UnTagLeft_Hand()
    //{
    //    left_Hand_Attack_Point.tag = Tags.UNTAGGED_TAG;
    //}

    //void TagLeft_Leg()
    //{
    //    left_Leg_Attack_Point.tag = Tags.LEFT_LEG_TAG;
    //}

    //void UnTagLeft_Leg()
    //{
    //    left_Leg_Attack_Point.tag = Tags.UNTAGGED_TAG;
    //}
    void TagStun()
    {
        character.tag = Tags.STUN_TAG;
    }

    void UnTagStunE()
    {
        character.tag = Tags.ENEMY_TAG;
    }

    void Enemy_StandUp()
    {
        StartCoroutine(StandUpAfterTimer());
    }

    IEnumerator StandUpAfterTimer()
    {
        yield return new WaitForSeconds(stand_Up_Timer);
        
        if(gameObject.transform.parent == null)
            animationScript.StandUp();

        if (gameObject.transform.parent != null)
            animationScript.Lift();
    }
}

