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

        Vector3 lookTarget = new Vector3(playerPos.x, spawnPos.y, playerPos.z); // monster faces player
        monster.transform.LookAt(lookTarget);

        monster.SetActive(true);
    }

    public static void UpdateSpawnInterval(int itemCount)
    {
        float interval = Mathf.Max(5f, itemCount * 5f); // getting interval value by updating items amount * seconds subtracted
        Debug.Log($"monster spawn interval updated to {interval} seconds. " +
            $"items collected: {itemCount})");
    }
}
