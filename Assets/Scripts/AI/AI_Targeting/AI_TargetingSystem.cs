// Roman Baranov 29.12.2021

using UnityEngine;

//[ExecuteInEditMode]
[RequireComponent(typeof(AI_Sensor))]
public class AI_TargetingSystem : MonoBehaviour
{
    #region VARIABLES
    [SerializeField] private float _memorySpan = 3.0f;
    [SerializeField] private float _distanceWeight = 1.0f;
    //[SerializeField] private float _angleWeight = 1.0f;
    //[SerializeField] private float _ageWeight = 1.0f;

    public bool HasTarget { get { return _bestMemory != null; } }
    public GameObject Target { get { return _bestMemory.gameObject; } }
    public Vector3 TargetPosition { get { return _bestMemory.gameObject.transform.position; } }
    public bool TargetInSight { get { return _bestMemory.Age < 0.5f/* seconds*/; } }
    public float TargetDistance { get { return _bestMemory.distance; } }

    private AI_SensoryMemory _memory = new AI_SensoryMemory(10);
    private AI_Sensor _sensor = null;
    private AI_Memory _bestMemory = null;
    #endregion

    #region UNITY Methods
    // Start is called before the first frame update
    void Start()
    {
        _sensor = GetComponent<AI_Sensor>();
    }

    // Update is called once per frame
    void Update()
    {
        _memory.UpdateSenses(_sensor);
        _memory.ForgetMemories(_memorySpan);

        EvaluateScores();
    }
    #endregion

    #region PRIVATE Methods
    private void EvaluateScores()
    {
        _bestMemory = null;

        foreach (var memory in _memory.memories)
        {
            memory.score = CalculateScore(memory);
            if (_bestMemory == null ||
                memory.score > _bestMemory.score)
            {
                _bestMemory = memory;
                //Debug.Log(_bestMemory.gameObject.name);
            }
        }
    }

    private float Normalize(float value, float maxValue)
    {
        return 1.0f - (value / maxValue);
    }

    /// <summary>
    /// Calculates score for the given target
    /// </summary>
    /// <param name="memory">Target to calculate score</param>
    /// <returns>Score value</returns>
    private float CalculateScore(AI_Memory memory)
    {
        float distanceScore = Normalize(memory.distance, _sensor.Distance) * _distanceWeight;
        //float angleScore = Normalize(memory.angle, _sensor.Angle) * _angleWeight;
        //float ageScore = Normalize(memory.Age, _memorySpan) * _ageWeight;
        //float targetPriority = Normalize(memory.trgetPriority, TargetPriority.Depth);
        //return distanceScore + angleScore + ageScore + targetPriority;
        return distanceScore;
    }
    #endregion

    #region DEBUG
    private void OnDrawGizmos()
    {
        float maxScore = float.MinValue;
        foreach (var memory in _memory.memories)
        {
            maxScore = Mathf.Max(maxScore, memory.score);
        }

        foreach (var memory in _memory.memories)
        {
            Color color = Color.red;
            if (memory == _bestMemory)
            {
                color = Color.blue;
            }
            color.a = memory.score / maxScore;
            Gizmos.color = color;
            Gizmos.DrawSphere(memory.position, 0.2f);
        }
    }
    #endregion
}
