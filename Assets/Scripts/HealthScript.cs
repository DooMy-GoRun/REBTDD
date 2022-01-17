using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HealthScript : MonoBehaviour
{
    [SerializeField] private float health = 100f;

    private HealthUI health_UI;
    
    private CharacterAnimation animationChar;
    [SerializeField] private float healthToSuper = 4f;
    
    [SerializeField] private bool is_Player;

    private GameObject liftOff;

    private void Awake()
    {
        animationChar = GetComponentInChildren<CharacterAnimation>();

        if(is_Player)
            health_UI = GetComponent<HealthUI>();
    }

    public void ApplyDamage(float damage, bool knockDown)
    {
        liftOff = GameObject.FindGameObjectWithTag("Stun");

        if (is_Player)
        {
            health -= damage;
            health_UI.DisplayHealth(health);

            if (health <= 0f)
            {
                animationChar.Death();

                if (liftOff != null)
                {
                    if (liftOff.transform.parent != null)
                    {
                        liftOff.transform.parent = null;

                        animationChar.LiftOFF();
                    }
                }
                //return;

            }
            if (knockDown)
            {
                gameObject.layer = LayerMask.NameToLayer("DownLayer");

                if (liftOff != null)
                {
                    if (liftOff.transform.parent != null)
                    {
                        liftOff.transform.parent = null;

                        animationChar.LiftOFF();
                    }
                }

                animationChar.KnockDown();

            }
            else
            {
 
                animationChar.Hit();
                    

                if (liftOff != null)
                {
                    if (liftOff.transform.parent != null)
                    {
                        animationChar.LiftON();
                    }
                    else if (liftOff.transform.parent == null)
                    {
                        animationChar.LiftOFF();
                        liftOff.transform.parent = null;
                    }
                }
            }
        }
        
        if(!is_Player)
        {
            if (gameObject.layer == LayerMask.NameToLayer("Death"))
                return;

            health -= damage;

            if (health <= healthToSuper)
            {
                gameObject.layer = LayerMask.NameToLayer("LayerToSuper");
            }


            //display health UI
            if (health <= 0f)
            {
                animationChar.Death();
                return;

            }
            if (knockDown)
            {
                animationChar.KnockDown();
            }
            else
            {
                animationChar.Hit();
            }
            
        }
    }
}
