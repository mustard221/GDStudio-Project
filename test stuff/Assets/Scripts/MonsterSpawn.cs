using UnityEngine;
using System.Collections;

public class MonsterSpawn : MonoBehaviour
{
    public GameObject monster;
    public Character2 player;

    public float initialDelay = 3f;
    public float baseInterval = 30f;
    public float minInterval = 5f;
    
    private float spawnInterval;
    private Coroutine spawnRoutine;

    private static MonsterSpawn instance;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        spawnInterval = baseInterval; // initialize spawn interval

        if (monster != null)
            monster.SetActive(false); // ensure single monster is hidden until first spawn

        spawnRoutine = StartCoroutine(spawnLoop(initialDelay)); // using coroutine to create spawn loop
    }

    private IEnumerator spawnLoop(float delay)
    {
        yield return new WaitForSeconds(delay);

        while (true)
        {
            spawnOnce();
            yield return new WaitForSeconds(spawnInterval); // wait for next spawn
        }
    }

    private void spawnOnce()
    {
        Vector3 playerPos = player.transform.position; // get player position to determine spawn location

        // generate random distance within range
        Vector2 randomDir = Random.insideUnitCircle.normalized;
        float randomDist = Random.Range(4f, 8f);

        // calculate spawn position using the random direction & distance values
        Vector3 spawnOffset = new Vector3(randomDir.x, 1.05f, 1f) * randomDist;
        Vector3 spawnPos = playerPos + spawnOffset;

        // spawn & position monster towards player
        monster.transform.position = spawnPos; 
        monster.transform.LookAt(playerPos);
        monster.SetActive(true);

        // initiate despawn coroutine
        StartCoroutine(Despawn());
    }

    private IEnumerator Despawn()
    {
        yield return new WaitForSeconds(6f); // disappears after 6 seconds
        monster.SetActive(false);
    }

    // updates spawn interval based on collected items
    public static void updateInterval(int itemCount)
    {
        float newInterval = Mathf.Max(instance.minInterval, instance.baseInterval - itemCount * 5f); // decrease interval by 5 secs per item, min capped
        instance.newInterval(newInterval, itemCount);
    }

    private void newInterval(float newInterval, int itemCount) // method to update interval based on the interval & item amount values
    {
        if (Mathf.Approximately(newInterval, spawnInterval)) return;

        spawnInterval = newInterval; 
        Debug.Log($"spawn interval updated to {spawnInterval} secs. items collected: {itemCount}");
    }
}
