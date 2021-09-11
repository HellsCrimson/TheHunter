using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMoov : MonoBehaviour
{

    private const float Speed = 0.02f;
    private Vector3 direction;

    void Start()
    {
        direction = (new Vector3(Random.Range(-0.02f, 0.02f), 0.0f, Random.Range(-0.02f, 0.02f))).normalized;
    }

    void Update()
    {
        transform.position += (direction * Speed);
    }


    void OnCollisionEnter(Collision hit)

    {
        Vector3 dir = new Vector3(Random.Range(-0.02f, 0.02f), 0.0f, Random.Range(-0.02f, 0.02f));
        direction = dir.normalized;
    }
    
}