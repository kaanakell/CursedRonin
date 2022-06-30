using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    PlayerHealth playerHealth;

    public float healthBonus = 15f;

    void Awake()
    {
        playerHealth = FindObjectOfType<PlayerHealth>();
    }

    private void OnParticleTrigger()
    {
        if (playerHealth.currentHealth < playerHealth.maxHealth)
        {
            Debug.Log("HP");
            playerHealth.currentHealth += healthBonus;
        }
    }
}
