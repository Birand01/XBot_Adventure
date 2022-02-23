using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] meteorPrefabs;
    [SerializeField] int meteorCount;
    [SerializeField] float spawnDelay;

    GameObject[] meteors;
    void Start()
    {
        PrepareMeteors();
        StartCoroutine(SpawnMeteors());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnMeteors()
    {
        for (int i = 0; i < meteorCount; i++)
        {
            meteors[i].SetActive(true);
            yield return new WaitForSeconds(spawnDelay);
        }
    }
    void PrepareMeteors()
    {
        meteors=new GameObject[meteorCount];
        int prefabsCount=meteorPrefabs.Length;
        for (int i = 0; i < meteorCount; i++)
        {
            meteors[i]=Instantiate(meteorPrefabs[Random.Range(0,prefabsCount)],transform);
            meteors[i].SetActive(false);
        }
    }
}
