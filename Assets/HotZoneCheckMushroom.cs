using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotZoneCheckMushroom : MonoBehaviour
{
    private EnemyBehaviourMushroom enemyMushroomParent;
    private bool inRange;
    [SerializeField] Animator animator;

    private void Awake()
    {
        enemyMushroomParent = GetComponentInParent<EnemyBehaviourMushroom>();
    }

    private void Update()
    {
        if (inRange && !animator.GetCurrentAnimatorStateInfo(0).IsName("Mushroom_Attack"))
        {
            enemyMushroomParent.Flip();
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            inRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            inRange = false;
            gameObject.SetActive(false);
            enemyMushroomParent.triggerArea.SetActive(true);
            enemyMushroomParent.inRange = false;
            enemyMushroomParent.SelectTarget();
        }
    }
}
