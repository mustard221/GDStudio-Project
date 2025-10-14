using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MonsterSpawn : MonoBehaviour
{

    public GameObject monster;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("SpawnDelay", 3);
    }

    private void SpawnDelay()
    {
        monster.SetActive(true);
    }

    public static void UpdateSpawnInterval(int itemCount)
    {
        float interval = Mathf.Max(5f, 30f - itemCount * 5f); // interval of 5s
        Debug.Log($"Monster spawn interval updated: {interval} seconds (Items collected: {itemCount})"); // logging if updating correctly
    }
}
