using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    public GameObject newHeart;
    public float respawnTime = 1.0f;

    void Start()
    {
        StartCoroutine(healthPick());
        
    }

    public void SpawnNewHeart()
    {
        GameObject a = Instantiate(newHeart, transform.position, Quaternion.identity) as GameObject;
    }

    IEnumerator healthPick()
    {
        while (true)
        {
            yield return new WaitForSeconds(respawnTime);
            SpawnNewHeart();

        }
    }
}
