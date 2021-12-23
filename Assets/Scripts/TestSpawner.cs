using UnityEngine;
using UnityEngine.AI;

public class TestSpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyTemplate;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private float spawnTimer = 7f;
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private int maxCount = 7;

    private float currentTimer;

    private void Start()
    {
        agent.enabled = true;
    }

    private void Update()
    {
        if (maxCount <= 0)
            return;

        if (currentTimer > 0)
        {
            currentTimer -= Time.deltaTime;
            return;
        }

        if (maxCount > 0)
        {
            currentTimer = spawnTimer;
            maxCount--;
            CreateNewEnemy();
        }
    }

    private void CreateNewEnemy()
    {
        var newEnemy = Instantiate(enemyTemplate, spawnPosition.position, Quaternion.identity);
        newEnemy.SetActive(true);
    }
}