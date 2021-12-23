using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : MonoBehaviour
{
    [SerializeField] private float health = 100f;

    private CharacterAnimation animationChar;
    [SerializeField] private float healthToSuper = 4f;
    
    [SerializeField] private bool is_Player;

    private void Awake()
    {
        animationChar = GetComponentInChildren<CharacterAnimation>();
    }

    public void ApplyDamage(float damage, bool knockDown)
    {
        

        if(is_Player)
        {
            return;
        }
        
        if(!is_Player)
        {
            if (gameObject.layer == LayerMask.NameToLayer("Death"))
                return;

            health -= damage;

            if (health <= healthToSuper)
                gameObject.layer = LayerMask.NameToLayer("LayerToSuper");


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
