using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundCheck : MonoBehaviour
{
    PlayerController playerController;

    private void Awake()
    {
        playerController = GetComponentInParent<PlayerController>();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == playerController.gameObject)
            return;
        
        playerController.SetGroudedState(true);
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject == playerController.gameObject)
            return;
        
        playerController.SetGroudedState(false);
    }

    void OnTriggerStay(Collider other)
    {
        if(other.gameObject == playerController.gameObject)
            return;
        
        playerController.SetGroudedState(true);
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject == playerController.gameObject)
            return;
        
        playerController.SetGroudedState(true);
    }

    private void OnCollisionExit(Collision other)
    {
        if(other.gameObject == playerController.gameObject)
            return;
        
        playerController.SetGroudedState(false);
    }

    private void OnCollisionStay(Collision other)
    {
        if(other.gameObject == playerController.gameObject)
            return;
        
        playerController.SetGroudedState(true);
    }
}
