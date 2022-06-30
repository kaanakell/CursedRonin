using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    private GameObject enemyTwoPrefab;
    [SerializeField]
    private GameObject enemyThreePrefab;

    [SerializeField]
    private float enemyInterval = 3.5f;
    [SerializeField]
    private float enemyTwoInterval = 7f;
    [SerializeField]
    private float enemyThreeInterval = 14f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(spawnEnemy(enemyInterval, enemyPrefab));
        StartCoroutine(spawnEnemy(enemyTwoInterval, enemyTwoPrefab));
        StartCoroutine(spawnEnemy(enemyThreeInterval, enemyThreePrefab));
    }

    private IEnumerator spawnEnemy(float interval, GameObject enemy)
    {
        yield return new WaitForSeconds(interval);
        GameObject newEnemy = Instantiate(enemy, new Vector3(Random.Range(-5f, 5), Random.Range(-6f, 6f), 0), Quaternion.identity);
        StartCoroutine(spawnEnemy(interval, enemy));
    }
}
