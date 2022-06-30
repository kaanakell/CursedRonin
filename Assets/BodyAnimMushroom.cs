using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyAnimMushroom : MonoBehaviour
{
    private EnemyBehaviourMushroom enemyCooling;


    // Start is called before the first frame update
    void Start()
    {
        enemyCooling = GetComponentInParent<EnemyBehaviourMushroom>();
    }
    public void Animation_Finished()
    {
        enemyCooling.Attack_Cooldown();
    }
}
