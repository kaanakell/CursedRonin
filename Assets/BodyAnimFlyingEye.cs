using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyAnimFlyingEye : MonoBehaviour
{
    private FlyingEnemy enemyCooling;


    // Start is called before the first frame update
    void Start()
    {
        enemyCooling = GetComponentInParent<FlyingEnemy>();
    }
    public void Animation_Finished()
    {
        enemyCooling.Attack_Cooldown();
    }
}
