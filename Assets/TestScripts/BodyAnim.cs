using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyAnim : MonoBehaviour
{
    private EnemyBehaviour enemyCooling;


    // Start is called before the first frame update
    void Start()
    {
        enemyCooling = GetComponentInParent<EnemyBehaviour>();
    }
    public void Animation_Finished()
    {
        enemyCooling.Attack_Cooldown();
    }

    
}
