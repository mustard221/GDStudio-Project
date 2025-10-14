using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawn : MonoBehaviour
{
    public GameObject monster;
    public Character2 player;

    void Start()
    {
        Invoke("SpawnDelay", 3);
    }

    private void SpawnDelay()
    {
        if (player == null)
        {
            Debug.LogWarning("player not set");
            return;
        }
        //getting player position value
        Vector3 playerPos = player.transform.position;

        //random ranges for generating spawn position from player
        Vector2 randomDir = Random.insideUnitCircle.normalized;
        float randomDist = Random.Range(20f, 25f);

        //using values from randomized ranges
        Vector3 spawnOffset = new Vector3(randomDir.x, 0, randomDir.y) * randomDist;
       
        Vector3 spawnPos = playerPos + spawnOffset; // taking player position + offset to generate spawn location
        monster.transform.position = spawnPos; 
         
        Vector3 lookTarget = new Vector3(playerPos.x, 0, playerPos.z); // monster faces player
        monster.transform.LookAt(lookTarget);

        monster.SetActive(true);
    }

    public static void UpdateSpawnInterval(int itemCount)
    {
        float interval = Mathf.Max(5f, 30f - itemCount * 5f);
        Debug.Log($"Monster spawn interval updated: {interval} seconds (Items collected: {itemCount})");
    }
}
