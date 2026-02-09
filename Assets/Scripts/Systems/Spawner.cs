using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private Vector2 spawnArea;
    [SerializeField] private int amount;
    [SerializeField] private bool autoSpawn = true;
    public List<GameObject> SpawnedInstances { get; private set; } = new List<GameObject>();

    void Start()
    {
        if (autoSpawn)
        {
            Spawn(amount, spawnArea, prefab);
        }
    }

    public void Spawn(int amount, Vector2 spawnArea, GameObject prefab)
    {
        for(int i = 0; i < amount; i++)
        {
            Vector3 pos = transform.position + new Vector3(Random.Range(.5f * -spawnArea.x, .5f * spawnArea.x), 0, Random.Range(.5f * -spawnArea.y, .5f * spawnArea.y));
            var instance = Instantiate(prefab, pos, Quaternion.identity);
            SpawnedInstances.Add(instance);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(spawnArea.x, 1, spawnArea.y));
    }
}
