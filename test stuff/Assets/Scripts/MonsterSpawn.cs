using UnityEngine;
using System.Collections;

public class MonsterSpawn : MonoBehaviour
{
    public GameObject monster;
    public Character2 player;

    public float initialDelay = 3f;
    public float baseInterval = 5f;
    public float minInterval = 5f;
    public float itemCount; 
    private float spawnInterval;
    private Coroutine spawnRoutine;

    private static MonsterSpawn instance;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        spawnInterval = baseInterval;

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
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void spawnOnce()
    {
        Vector3 playerPos = player.transform.position;

        // get random distance & face towards player
        Vector2 randomDir = Random.insideUnitCircle.normalized;
        float randomDist = Random.Range(5f, 10f);
        Vector3 spawnOffset = new Vector3(randomDir.x, 0f, randomDir.y) * randomDist;
        Vector3 spawnPos = playerPos + spawnOffset;

        monster.transform.position = spawnPos;
        monster.transform.LookAt(playerPos);
        monster.SetActive(true);

        StartCoroutine(Despawn());
    }

    private IEnumerator Despawn()
    {
        yield return new WaitForSeconds(10f); // disappears after 6 seconds
        monster.SetActive(false);
    }

    // updates spawn interval based on collected items
    public static void updateInterval(int itemCount)
    {
        float newInterval = Mathf.Max(instance.minInterval, instance.baseInterval - itemCount * 5f);
        instance.newInterval(newInterval, itemCount);
    }

    private void newInterval(float newInterval, int itemCount)
    {
        if (Mathf.Approximately(newInterval, spawnInterval)) return;

        spawnInterval = newInterval;
        Debug.Log($"spawn interval updated to {spawnInterval} secs. items collected: {itemCount}");
    }
}
