using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;

public class ZombieManager : MonoBehaviour
{
    public int damage = 8;
    public float maxHealth = 25;
    public float curHealth;
    private AIEnnemi Distance;
    private NavMeshAgent shotDistance;
    private bool CanAttack = true;
    private bool isDead = false;

    private Animator animator;

    [SerializeField] private AudioSource bruit;

    [SerializeField] private MoveToTarget moveToTarget;
    private Rigidbody rb;
    private Collider _collider;

    private void Start()
    {
        curHealth = maxHealth;
        Distance = GetComponent<AIEnnemi>();
        shotDistance = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        
    }
    
    // Update is called once per frame
    void Update()
    {
        if (curHealth <= 0 && !isDead)
        {
            bruit.Stop();
            Distance.enabled = false;
            moveToTarget.enabled = false;
            shotDistance.enabled = false;
            isDead = true;
            animator.SetBool("death", true);
            rb.isKinematic = true;
            _collider.enabled = false;
            Destroy(gameObject, 1.5f);
            GameObject.Find("Game Controller").GetComponent<Survival>().Killed();
        }

        if (Distance.Distance <= shotDistance.stoppingDistance && CanAttack && !isDead)
        {
            animator.SetTrigger("attack");
            DealsDamage(damage);
            CanAttack = false;
            StartCoroutine(Wait());
        }
        
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1f);
        animator.SetTrigger("attack");
        CanAttack = true;
    }
    public void DealsDamage(int damage)
    {
        GameObject.FindWithTag("Player").GetComponent<HealthBar>().curHealth -= damage;
        if (GameObject.FindWithTag("Player").GetComponent<HealthBar>().curHealth < 1 && !GameObject.FindWithTag("Player").GetComponent<HealthBar>().dead)
        {
            GameObject.FindWithTag("Player").GetComponent<HealthBar>().curHealth = 0;
            GameObject.FindWithTag("Player").GetComponent<HealthBar>().dead = true;
            GameObject.FindWithTag("Player").GetComponent<PlayerControllerSolo>().Die();
        }
    }
    
    public void NextGen()
    {
        maxHealth += 5;
        curHealth = maxHealth;
        damage += 2;
        GameObject.FindWithTag("Player").GetComponent<HealthBar>().curHealth += 30;
        if (GameObject.FindWithTag("Player").GetComponent<HealthBar>().curHealth > 100)
            GameObject.FindWithTag("Player").GetComponent<HealthBar>().curHealth = 100;
    }
}
