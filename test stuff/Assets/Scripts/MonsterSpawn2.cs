using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//spawn monster at beginning, chase player, increase speed with item pickups

public class MonsterSpawn2 : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject monster;
    public Character2 player;

    public float monsterSpeed = 2f;
    public float baseSpeed = 5f;
    public float minSpeed = 1f;

    public float itemCount;

    private GameObject ground;    
    private static MonsterSpawn2 instance;

    void Awake()
    {
        instance = this;
    }
  
    void Start()
    {
        if (monster != null)
            monster.SetActive(false); // ensure single monster is hidden until spawn

        spawnOnce();
    }

    private void spawnOnce()
    {
        Vector3 playerPos = player.transform.position; // get player position to determine spawn location

        // calculate spawn position using offset + player position
        Vector3 spawnOffset = new Vector3(10f, 1f, 10f);
        Vector3 spawnPos = playerPos + spawnOffset;

        // monster faces towards player
        monster.transform.position = spawnPos;
        monster.transform.LookAt(playerPos);
        monster.SetActive(true);

        Debug.Log("monster spawned at " + spawnPos); // debug for spawn position

    }

    public static void updateSpeed(int itemCount)
    {
        float newSpeed = Mathf.Max(instance.minSpeed, instance.baseSpeed - itemCount * 2f); // calculating speed with item count + doesn't go below min speed
        instance.newSpeed(newSpeed, itemCount); // update speed
    }

    private void newSpeed(float newSpeed, int itemCount)
    {
        if (Mathf.Approximately(newSpeed, monsterSpeed)) return; // if speed is the same, do nothing

        monsterSpeed = newSpeed;
        Debug.Log($"monster speed updated to {monsterSpeed}. items collected: {itemCount}"); // debug for speed updates
    }

    private void Update() 
    {
        float move = monsterSpeed * Time.deltaTime;
        float yPos = player.transform.position.y;
        float yOffset = yPos + 0.4f;

        monster.transform.position = Vector3.MoveTowards(monster.transform.position, player.transform.position, move); // move to player location
        monster.transform.position = new Vector3(monster.transform.position.x, yOffset, monster.transform.position.z); // keep monster at player Y level
        monster.transform.LookAt(player.transform.position); // face player at all times

        // trying ground detection
      /*  if (ground.CompareTag("ground"))
        {
            float groundY = ground.transform.position.y;
            monster.transform.position = new Vector3(monster.transform.position.x, groundY + 1f, monster.transform.position.z);
        } */
      
    }
}
