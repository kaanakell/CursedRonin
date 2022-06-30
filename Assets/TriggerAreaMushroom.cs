using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAreaMushroom : MonoBehaviour
{
    private EnemyBehaviourMushroom enemyMushroomParent;

    private void Awake()
    {
        enemyMushroomParent = GetComponentInParent<EnemyBehaviourMushroom>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            enemyMushroomParent.target = collider.transform;
            enemyMushroomParent.inRange = true;
            enemyMushroomParent.hotZone.SetActive(true);
            Debug.Log("IN AREA");
        }
    }
}
