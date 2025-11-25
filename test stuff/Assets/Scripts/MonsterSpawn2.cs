using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//spawn monster at beginning, chase player, increase speed with item pickups

public class MonsterSpawn2 : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject monster;
    public Character2 player;

    public Rigidbody monsterRB;
    public Transform playerT;

    public float monsterSpeed = 2f;
    public float baseSpeed = 5f;
    public float minSpeed = 1f;

    public float itemCount;

    public static MonsterSpawn2 instance;

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
        if ((player == null && playerT == null) || monster == null)
        {
            Debug.LogWarning("MonsterSpawn2 missing player or monster reference.");
            return;
        }

        Vector3 playerPos = (player != null) ? player.transform.position : playerT.position; // get player position to determine spawn location

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
        if (instance == null)
            return;

        float newSpeed = Mathf.Max(instance.minSpeed, instance.baseSpeed - itemCount * 2f); // calculating speed with item count
        instance.newSpeed(newSpeed, itemCount); // update speed
    }

    private void newSpeed(float newSpeed, int itemCount)
    {
        if (Mathf.Approximately(newSpeed, monsterSpeed)) return; // if speed is the same, do nothing
        monsterSpeed = newSpeed;

        Debug.Log($"monster speed updated to {monsterSpeed}. items collected: {itemCount}"); // debug for speed updates
    }

    private void FixedUpdate()
    {
        if (monster == null || monsterRB == null || playerT == null)
            return; // check references

        // taking current monster position
        Vector3 monsterPos = monster.transform.position; 
        Vector3 toPlayer = playerT.position - monsterPos;
        
        // get direction and distance to player 
        float distance = toPlayer.magnitude;
        if (distance < Mathf.Epsilon) return;
        Vector3 targetDir = toPlayer.normalized;

        // chase slower if no line of sight
        bool hasLOS = false;
        if (distance <= 50f) // only raycast if reasonably close and check for player tag
        {
            if (Physics.Raycast(monsterPos + Vector3.up * 0.5f, targetDir, out RaycastHit hit, 50f))
            {
                if (hit.transform == playerT || hit.transform.CompareTag("Player"))
                    hasLOS = true;
            }
        }

        // movement + delta time to keep speed consistent
        float speedMultiplier = hasLOS ? 1f : 0.6f; // slower if no clear line of sight (tweakable)
        Vector3 newPos = monsterPos + targetDir * monsterSpeed * speedMultiplier * Time.fixedDeltaTime;
        monsterRB.MovePosition(newPos);

        // face towards player smoothly w/ slerp
        Quaternion targetRot = Quaternion.LookRotation(targetDir, Vector3.up);
        monsterRB.MoveRotation(Quaternion.Slerp(monster.transform.rotation, targetRot, 10f * Time.fixedDeltaTime));
    }
}
