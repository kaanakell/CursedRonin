using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeleport : MonoBehaviour
{
    [HideInInspector]
    public bool hasHit;

    Rigidbody2D rb;

    [HideInInspector]
    public Transform kunai_child;

    void Start()
    {
        kunai_child = transform.GetChild(0);
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        //GameObject.FindGameObjectWithTag("Kunai").transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        hasHit = true;
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
    }
}
