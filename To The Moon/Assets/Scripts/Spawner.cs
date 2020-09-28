using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _prefabs;
    [SerializeField] private int _index = 0;
    [SerializeField] FloatRange timeBetweenSpawns, randomVelocity, randomGap, rx;
    private float currentSpawnDelay;
    private float timeSinceLastSpawn;

    private void FixedUpdate()
    {
        timeSinceLastSpawn += Time.deltaTime;
        if(timeSinceLastSpawn >= currentSpawnDelay)
        {
            timeSinceLastSpawn -= currentSpawnDelay;
            currentSpawnDelay = timeBetweenSpawns.RandomInRange;
            SpawnObject();
        }
    }

    private void SpawnObject()
    {
        GameObject prefab = _prefabs[_index];
        GameObject spawn = Instantiate(prefab);
        GameManager.Instance.AddEnemy(spawn.GetComponent<Interactive>());
        spawn.transform.localPosition = transform.position;
        spawn.transform.localPosition += new Vector3(rx.RandomInRange, randomGap.RandomInRange, 0);
    }
}
