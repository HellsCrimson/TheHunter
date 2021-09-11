using System.Collections;
using System.Collections.Generic;
using System.IO;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.AI;

public class AIEnnemi : MonoBehaviour
{
    public Transform Target;
    private NavMeshAgent AgentEnnemi;
    public float Distance; // 1
    public float WalkDistance; // 1 (distance a laquelle l'ennemi va nous suivre)
    private bool EnnemiIsDead = false;
    private Vector3 InitialPosition;

    [SerializeField] MoveToTarget _moveToTarget;
    void Start()
    {
        AgentEnnemi = GetComponent<NavMeshAgent>();
        InitialPosition = transform.position;
    }
    
    void Update()
    {
        if (Target == null)
            Target = GameObject.FindWithTag("Player").transform;
        if (Target == null)
            return;
        /*AgentEnnemi.SetDestination(Target.position);*/ // 2 (pour que ça suive tout le temps)

        Distance = Vector3.Distance(Target.position, transform.position); // 1 (récupere distance ennemi target)
        if (Distance < WalkDistance && !EnnemiIsDead)
        {
            _moveToTarget.enabled = false;
            AgentEnnemi.SetDestination(Target.position);
        }
        else
        {
            _moveToTarget.enabled = true;
            _moveToTarget.agent.destination = _moveToTarget.agentDestination[_moveToTarget.i].position;
            //AgentEnnemi.SetDestination(InitialPosition);
        }

    }
}
