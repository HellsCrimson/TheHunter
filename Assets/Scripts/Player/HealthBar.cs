using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private PhotonView PV;
    
    public static HealthBar Instance;
    
    PlayerController controller;
    
    public int maxHealth = 100;
    public int curHealth = 100;
    
    public Slider healthSlider;
    [SerializeField] TMP_Text textHealth;
    public Gradient healthGradient;
    public Image fill;

    public bool dead = false;

    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        PV = GetComponent<PhotonView>();
        controller = GetComponent<PlayerController>();
        
        healthSlider.maxValue = maxHealth;
        healthSlider.value = curHealth;
        fill.color = healthGradient.Evaluate(1f);
    }

    private void Update()
    {
        Health();
    }

    private void Health()
    {
        ValidHealth();
        healthSlider.value = curHealth;
        textHealth.text = curHealth + "/" + maxHealth;
        fill.color = healthGradient.Evaluate(healthSlider.normalizedValue);
    }

    private void ValidHealth()
    {
        if (curHealth > maxHealth)
            curHealth = maxHealth;
    }
    
    public void GetDamage(int damage)
    {
        if (PV.IsMine)
            curHealth -= damage;
        if (curHealth < 1)
            controller.Die();
    }
}
