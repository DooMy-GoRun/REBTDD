using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;

    [SerializeField] private GameObject[] m_Enemy;

    void Awake()
    {
        if(instance == null)
            instance = this;

    }

    public void SpawnEnemy()
    {
        var numberEnemy = Random.Range(0, 3);

        Instantiate(m_Enemy[numberEnemy], transform.position, Quaternion.identity);
    }
    
}
