using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    public int Limit = 5;
    public float delayToSpawn = 2f;
    public GameObject[] ennemy;
    public GameObject[] ennemiesAlive;
    private Vector2 SpawnPoint;
    private bool CanSpawn = true;

    void Start()
    {
        SpawnPoint = new (transform.position.x - 1f, transform.position.y);
        if(ennemiesAlive == null) ennemiesAlive = new GameObject[0];
    }

    // Update is called once per frame
    void Update()
    {
        if(ennemiesAlive.Length < Limit)
        {
            if (CanSpawn)
            {
                StartCoroutine(Spawn());
            }
        }
    }

    IEnumerator Spawn()
    {
        int choice = Random.Range(0, ennemy.Length);

        GameObject spawned = GameObject.Instantiate(ennemy[choice], SpawnPoint, Quaternion.identity);

        ennemiesAlive = ennemiesAlive.Append(spawned).ToArray();
        CanSpawn = false;
        yield return new WaitForSeconds(delayToSpawn);
        CanSpawn = true;
    }

    void DeadEnemy(GameObject dead)
    {
        ennemiesAlive = ennemiesAlive.Where(e => e != dead).ToArray();
    }


}
