using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationDelegate : MonoBehaviour
{
    [Header("Points attack")]
    [SerializeField] private GameObject left_Hand_Attack_Point; 
    [SerializeField] private GameObject right_Hand_Attack_Point;
    [SerializeField] private GameObject left_Leg_Attack_Point;
    [SerializeField] private GameObject right_Leg_Attack_Point;
    [SerializeField] private GameObject head_Attack_Point;
    [Space(3)]
    [SerializeField] private GameObject characterToStun;


    private float stand_Up_Timer = 1.5f;

    private CharacterAnimation animationScript;

    private AudioSource audioSource;

    [Space(10)]
    [Header("Sound effects")]
    [SerializeField] private AudioClip attack_sound;
    [SerializeField] private AudioClip finish_attack_sound;
    [SerializeField] private AudioClip super_attack_sound;
    [SerializeField] private AudioClip jump_sound;
    [SerializeField] private AudioClip jump_standup_sound;
    [SerializeField] private AudioClip grab_sound;
    [SerializeField] private AudioClip head_sound;
    [SerializeField] private AudioClip knock_sound;
    [SerializeField] private AudioClip dead_sound;

    private ShakeCamera shakeCamera;

    private void Awake()
    {
        animationScript = GetComponent<CharacterAnimation>();
        audioSource = GetComponent<AudioSource>();
        shakeCamera = GameObject.FindWithTag(Tags.MAIN_CAMERA_TAG).GetComponent<ShakeCamera>();
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
        left_Hand_Attack_Point.tag = Tags.THROW_HAND_TAG;
    }

    void UnTagThrow()
    {
        left_Hand_Attack_Point.tag = Tags.UNTAGGED_TAG;
    }

    void TagZero_Hand()
    {
        right_Hand_Attack_Point.tag = Tags.ZERO_HAND_TAG;
    }

    void UnTagZero_Hand()
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
    void TagFinish1E_Hand()
    {
        right_Hand_Attack_Point.tag = Tags.FINISH1_HAND_TAG;
    }
    void UnTagFinish1E_Hand()
    {
        right_Hand_Attack_Point.tag = Tags.UNTAGGED_TAG;
    }


    void TagComboE_Hand()
    {
        left_Hand_Attack_Point.tag = Tags.COMBO_HAND_TAG;
    }
    void UnTagComboE_Hand()
    {
        left_Hand_Attack_Point.tag = Tags.UNTAGGED_TAG;
    }

    void TagFinishE_Hand()
    {
        left_Hand_Attack_Point.tag = Tags.FINISH_HAND_TAG;
    }
    void UnTagFinishE_Hand()
    {
        left_Hand_Attack_Point.tag = Tags.UNTAGGED_TAG;
    }


    void TagStun()
    {
        characterToStun.tag = Tags.STUN_TAG;
    }

    void UnTagStunE()
    {
        characterToStun.tag = Tags.ENEMY_TAG;
    }

    void Enemy_StandUp()
    {
        StartCoroutine(StandUpAfterTimer());
    }

    void Toads_StandUp()
    {
        StartCoroutine(StandUpToads());
    }

    IEnumerator StandUpAfterTimer()
    {
        yield return new WaitForSeconds(stand_Up_Timer);
       
            animationScript.StandUp();
    }

    IEnumerator StandUpToads()
    {
        yield return new WaitForSeconds(stand_Up_Timer - 1f);
        animationScript.StandUp();
    }


    //sounds fx

    void Attack_Sound()
    {
        Ray ray = new Ray(transform.position + new Vector3(0, 0.5f, 0), transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.distance < 2 && hit.collider.gameObject.layer != LayerMask.NameToLayer("Ignore Raycast") && hit.collider.gameObject.layer != LayerMask.NameToLayer("Default"))
            {
                audioSource.volume = 0.1f;
                audioSource.clip = attack_sound;
                audioSource.Play();
            }
        }
    }

    void Finish_Attack_Sound()
    {
        Ray ray = new Ray(transform.position + new Vector3(0, 0.5f, 0), transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.distance < 2.5 && hit.collider.gameObject.layer != LayerMask.NameToLayer("Ignore Raycast") && hit.collider.gameObject.layer != LayerMask.NameToLayer("Default"))
            {
                audioSource.volume = 0.2f;
                audioSource.clip = finish_attack_sound;
                audioSource.Play();
            }
        }
    }

    void Super_Attack_Sound()
    {
        audioSource.volume = 0.2f;
        audioSource.clip = super_attack_sound;
        audioSource.Play();
    }

    void Knock_Sound()
    {
        audioSource.volume = 0.5f;
        audioSource.clip = knock_sound;
        audioSource.Play();
    }

    void Head_Sound()
    {
        audioSource.volume = 0.15f;
        audioSource.clip = head_sound;
        audioSource.Play();
    }

    void Jump_Sound()
    {
        audioSource.volume = 0.3f;
        audioSource.clip = jump_sound;
        audioSource.Play();
    }

    void Jump_Standup_Sound()
    {
        audioSource.volume = 0.3f;
        audioSource.clip = jump_standup_sound;
        audioSource.Play();
    }

    void Grab_Sound()
    {
        audioSource.volume = 0.3f;
        audioSource.clip = grab_sound;
        audioSource.Play();
    }

    void Died_Sound()
    {
        audioSource.volume = 0.2f;
        audioSource.clip = dead_sound;
        audioSource.Play();
    }

    //shake camera
    void ShakeCameraOnFall()
    {
        shakeCamera.ShouldShake = true;
    }

    //enemy dead

    //void CharacterDied()
    //{
    //    Invoke("DeactivateGameObject", 2f);
    //}

    //void DeactivateGameObject()
    //{
    //    EnemyManager.instance.SpawnEnemy();

    //    gameObject.SetActive(false);
    //}
}

