// Roman Baranov 29.12.2021

using System.Collections.Generic;
using UnityEngine;

public class AI_Memory
{
    #region VARIABLES
    public float Age { get { return Time.time - lastSeen; } }

    public GameObject gameObject = null;
    public Vector3 position;
    public Vector3 direction;
    public float distance;
    public float angle;
    public float lastSeen;
    public float score;
    #endregion
}
public class AI_SensoryMemory
{
    #region VARIABLES
    public List<AI_Memory> memories = new List<AI_Memory>();

    private GameObject[] _characters;
    #endregion

    #region CONSTRUCTOR
    public AI_SensoryMemory(int maxPlayers)
    {
        _characters = new GameObject[maxPlayers];
    }
    #endregion

    #region PUBLIC Methods

    /// <summary>
    /// Put targets int the sensor memory
    /// </summary>
    /// <param name="sensor"></param>
    public void UpdateSenses(AI_Sensor sensor)
    {
        int targets = sensor.Filter(_characters, "Player");
        for (int i = 0; i < targets; i++)
        {
            GameObject target = _characters[i];
            RefreshMemory(sensor.gameObject, target);
        }
    }

    /// <summary>
    /// Update target parameters stored in the memory
    /// </summary>
    /// <param name="agent"></param>
    /// <param name="target"></param>
    public void RefreshMemory(GameObject agent, GameObject target)
    {
        AI_Memory memory = FetchMemory(target);
        memory.gameObject = target;
        memory.position = target.transform.position;
        memory.direction = target.transform.position - agent.transform.position;
        memory.distance = memory.direction.magnitude;
        memory.angle = Vector3.Angle(agent.transform.forward, memory.direction);
        memory.lastSeen = Time.time;
    }

    /// <summary>
    /// Search for game object in the memory and return it
    /// </summary>
    /// <param name="gameObject"></param>
    /// <returns></returns>
    public AI_Memory FetchMemory(GameObject gameObject)
    {
        AI_Memory memory = memories.Find(x => x.gameObject == gameObject);
        if (memory == null)
        {
            memory = new AI_Memory();
            memories.Add(memory);
        }

        return memory;
    }

    /// <summary>
    /// Remove object from memory that is older than passed in as parameter
    /// </summary>
    /// <param name="olderThan">Time parameter to filter objects by</param>
    public void ForgetMemories(float olderThan)
    {
        memories.RemoveAll(m => m.Age > olderThan);
        memories.RemoveAll(m => !m.gameObject);
        //TO DO check if target is dead
        //memories.RemoveAll(m => m.gameObject.GetComponent<Health>().IsDead());
    }
    #endregion
}
