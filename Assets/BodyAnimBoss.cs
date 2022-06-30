using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyAnimBoss : MonoBehaviour
{
    private BossWeapon bossCooling;


    // Start is called before the first frame update
    void Start()
    {
        bossCooling = GetComponent<BossWeapon>();
    }
    public void Animation_Finished()
    {
        bossCooling.Attack_Cooldown();
    }
}
