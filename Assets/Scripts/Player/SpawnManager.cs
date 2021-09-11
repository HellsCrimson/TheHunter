using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;

    SpawnPoint[] spawnpoints;

    private int index = 0;
    
    void Awake()
    {
        Instance = this;
        spawnpoints = GetComponentsInChildren<SpawnPoint>();
    }

    public Transform GetSpawnPoint()
    {
        return spawnpoints[Random.Range(0, spawnpoints.Length)].transform;
    }

    public Transform GetSpawnPointOrdered()
    {
        Transform spawn = spawnpoints[index].transform;
        if (index + 1 > spawnpoints.Length - 1)
            index = 0;
        else
            index++;
        
        return spawn;
    }
}
