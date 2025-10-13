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
}
