using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackUniversal : MonoBehaviour
{
    [SerializeField] private LayerMask collisionLayer;
    [SerializeField] private float _radius = 1f;
    [SerializeField] private float _comboHand = 1f;
    [SerializeField] private float _finishHand = 2f;
    [SerializeField] private float _superHand = 4f;
    [SerializeField] private float _Head = 2f;
    [SerializeField] private float _superHead = 4f;
    [SerializeField] private float _superLeg = 8f;
    [SerializeField] private bool is_Player, is_Enemy;

    [SerializeField] private GameObject hit_FX;
    [SerializeField] private GameObject hit_FX2;


    void Update()
    {
        DetectCollision();
    }


    //detect collision for attack points
    void DetectCollision()
    {
        Collider[] hit = Physics.OverlapSphere(transform.position, _radius, collisionLayer);

        //if we have a hit
        if(hit.Length > 0)
        {
            if(is_Player)
            {
                Vector3 hitFX_Pos = hit[0].transform.position;
                hitFX_Pos.y += 1.3f;
                

                    if (hit[0].transform.forward.x > 0)
                    {
                        hitFX_Pos.x += 0.3f;
                    }
                    else if (hit[0].transform.forward.x < 0)
                        hitFX_Pos.x -= 0.3f;

                    //Instantiate(hit_FX, hitFX_Pos, Quaternion.identity);

                    if (gameObject.CompareTag(Tags.HEAD_TAG))
                    {
                        hit[0].GetComponent<HealthScript>().ApplyDamage(_Head, true);
                        Instantiate(hit_FX, hitFX_Pos, Quaternion.identity);
                    }
                    else
                    {
                        hit[0].GetComponent<HealthScript>().ApplyDamage(0f, false);
                    }

                    if (gameObject.CompareTag(Tags.COMBO_HAND_TAG))
                    {
                        hit[0].GetComponent<HealthScript>().ApplyDamage(_comboHand, false);
                    }
                    else
                    {
                        hit[0].GetComponent<HealthScript>().ApplyDamage(0f, false);
                    }

                    if (gameObject.CompareTag(Tags.FINISH_HAND_TAG))
                    {
                        hit[0].GetComponent<HealthScript>().ApplyDamage(_finishHand, true);
                        Instantiate(hit_FX, hitFX_Pos, Quaternion.identity);
                    }
                    else
                    {
                        hit[0].GetComponent<HealthScript>().ApplyDamage(0f, false);
                    }

                    if (gameObject.CompareTag(Tags.SUPER_HAND_TAG))
                    {
                        hit[0].GetComponent<HealthScript>().ApplyDamage(_superHand, false);
                        Instantiate(hit_FX2, hitFX_Pos, Quaternion.identity);
                    }

                    if (gameObject.CompareTag(Tags.SUPER_LEG_TAG))
                    {
                        hit[0].GetComponent<HealthScript>().ApplyDamage(_superLeg, false);
                        Instantiate(hit_FX2, hitFX_Pos, Quaternion.identity);
                    }

                    if (gameObject.CompareTag(Tags.SUPER_HEAD_TAG))
                    {
                        hit[0].GetComponent<HealthScript>().ApplyDamage(_superHead, false);
                        Instantiate(hit_FX2, hitFX_Pos, Quaternion.identity);
                    }
                   

                if (gameObject.CompareTag(Tags.THROW_HAND_TAG))
                {
                    hit[0].GetComponent<HealthScript>().ApplyDamage(_finishHand, true);
                }
                else
                {
                    hit[0].GetComponent<HealthScript>().ApplyDamage(0f, false);
                }

                if(gameObject.CompareTag(Tags.ZERO_HAND_TAG))
                {
                    hit[0].GetComponent<HealthScript>().ApplyDamage(0f, false);
                }
            }

            if(!is_Player)
            {

                Vector3 hitFX_Pos = hit[0].transform.position;
                hitFX_Pos.y += 1.3f;


                if (hit[0].transform.forward.x > 0)
                {
                    hitFX_Pos.x += 0.3f;
                }
                else if (hit[0].transform.forward.x < 0)
                    hitFX_Pos.x -= 0.3f;

                if (gameObject.CompareTag(Tags.HEAD_TAG))
                    {
                        hit[0].GetComponent<HealthScript>().ApplyDamage(_Head, true);
                    Instantiate(hit_FX, hitFX_Pos, Quaternion.identity);

                }
                    else
                    {
                        hit[0].GetComponent<HealthScript>().ApplyDamage(0f, false);
                    }

                if (gameObject.CompareTag(Tags.FINISH1_HAND_TAG))
                {
                    hit[0].GetComponent<HealthScript>().ApplyDamage(1f, true);

                    Instantiate(hit_FX, hitFX_Pos, Quaternion.identity);
                }
                else
                {
                    hit[0].GetComponent<HealthScript>().ApplyDamage(0f, false);
                }


                if (gameObject.CompareTag(Tags.FINISH_HAND_TAG))
                    {
                        hit[0].GetComponent<HealthScript>().ApplyDamage(_finishHand, true);

                     Instantiate(hit_FX, hitFX_Pos, Quaternion.identity);
                    }
                    else
                    {
                        hit[0].GetComponent<HealthScript>().ApplyDamage(0f, false);
                    }

                    if (gameObject.CompareTag(Tags.COMBO_HAND_TAG))
                    {
                        hit[0].GetComponent<HealthScript>().ApplyDamage(_comboHand, false);
                    }
                    else
                    {
                        hit[0].GetComponent<HealthScript>().ApplyDamage(0f, false);
                    }

                if (gameObject.CompareTag(Tags.ZERO_HAND_TAG))
                {
                    hit[0].GetComponent<HealthScript>().ApplyDamage(0f, false);
                }
            }

            gameObject.SetActive(false);
        }
    }
}
