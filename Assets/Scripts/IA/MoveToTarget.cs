using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
using RandomC = System.Random;
using UnityEngine.UIElements;

public class MoveToTarget : MonoBehaviour
{
    public Transform[] agentDestination;
    public int i = 0;
    public NavMeshAgent agent;
    bool res;
    
    private void Start()
    {
        agentDestination = new Transform[5];
        var pathGameObjects = FindObjectsOfType<GameObject>();
        int j = 0;
        foreach (var transformGameObject in pathGameObjects)
        {
            if (transformGameObject.name.Contains("Path"))
            {
                agentDestination[j] = transformGameObject.transform;
                j++;
            }
        }
        agent = GetComponent<NavMeshAgent>();
        agent.destination = agentDestination[i].position;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.transform.position == agentDestination[i].position)
            res = true;
    }

    private void Update()
    {
        if (res)
        {
            var cur = i;
            while (cur == i)
            {
                i = Random.Range(0, 5);
            }

            agent.destination = agentDestination[i].position;
            res = false;
        }
    }
}
