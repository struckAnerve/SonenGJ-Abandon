using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

public class SpawnManager : MonoBehaviour {
	public GameObject spawnParent;
	public ObjectPooler objectPooler;
    public TerrainManager tm;

	float spawnDelay     =    1;
	float spIncr         = 0.1F;
	float spreadRadius   =    5;

	float angleSpread    = 80;

    float minRadius      = 60F;
    float radiusSpread   = 10F;

    //float minRadius = 5F;
    //float radiusSpread   = 20F;

    private GameObject player;
	float spawnTimer;

    int spawnMax = 20;
    int spawnCount;

	public SpawnableCollection collection;
    void OnEnable()
    {
            Debug.Log("HEI");
            Events.instance.AddListener<AbandonerChanged>(OnAbandonerChanged);
            Events.instance.AddListener<ObjectDespawned>(OnObjectDespawned);
    }

	// Use this for initialization
	void Start () { 
		collection = new SpawnableCollection();
		collection = collection.Load ("spawnables.xml");
		spawnTimer = spawnDelay;
	}
	
	// Update is called once per frame
	void Update () {
		spawnTimer -= Time.deltaTime;
        if (spawnCount <= spawnMax)
        {
            if (spawnTimer <= 0)
            {
                spawnTimer = spawnDelay;
                foreach (Spawnable spawnable in collection.spawnables)
                {
                    if (SpawnCheck(spawnable.initialSpawnProb, spawnable.failedSpawnCount))
                    {
                        Spawn(spawnable.name);
                        spawnable.failedSpawnCount = 0;
                    }
                    else
                    {
                        spawnable.failedSpawnCount += 1;
                    }
                }
            }
        }
	}
	bool SpawnCheck(float initialSpawnProb,int failedSpawnCount){
		float r = Random.value;
		float currentSpawnProb = initialSpawnProb + spIncr * failedSpawnCount;
		return (r<=currentSpawnProb);
	}
	void Spawn(string name){
		Vector3 camPos = Camera.main.transform.position;
        Vector3 playerPos = player.GetComponent<Rigidbody>().position;

        Quaternion playerRot = player.GetComponent<Rigidbody>().rotation;
        Quaternion randomrot = Quaternion.Euler(0, Random.Range(-angleSpread/2, angleSpread/2), 0);
        
		float spawnRad = Random.Range(minRadius, minRadius+radiusSpread);
        Vector3 spawnPos = playerRot * randomrot * Vector3.forward * spawnRad;

        Vector3 tempPos = spawnPos + playerPos + Vector3.up * 20;
        RaycastHit hit;
        Physics.Raycast(tempPos, Vector3.down, out hit);
        Vector3 finalPos = hit.point + Vector3.up * 4;
        GameObject spawned = objectPooler.Spawn(name, spawnParent.transform, finalPos, Quaternion.Euler(0, Random.Range(-90, 90), 0));
        spawnCount++;
    }

    private void OnAbandonerChanged(AbandonerChanged e)
    {
        player = e.newAbandoner;
    }

    private void OnObjectDespawned(ObjectDespawned e)
    {
        spawnCount--;
    }

    void OnDisable()
    {
        Events.instance.RemoveListener<AbandonerChanged>(OnAbandonerChanged);
        Events.instance.RemoveListener<ObjectDespawned>(OnObjectDespawned);
    }
}
