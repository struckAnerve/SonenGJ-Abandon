using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

public class SpawnManager : MonoBehaviour {
	public GameObject spawnParent;
	public ObjectPooler objectPooler;

	float spawnDelay     =    3;
	float spIncr         = 0.1F;
	float spreadRadius   =    5;

	float angleSpread    = 90;

    //float minRadius      = 52F;
    //float radiusSpread   = 30F;

    float minRadius = 5F;
    float radiusSpread   = 20F;

    public GameObject player;
	float spawnTimer;

	public SpawnableCollection collection;
	
	// Use this for initialization
	void Start () { 
		collection = new SpawnableCollection();
		collection = collection.Load ("spawnables.xml");
		Debug.Log (collection.spawnables[0].name);
		spawnTimer = spawnDelay;
	}
	
	// Update is called once per frame
	void Update () {
		spawnTimer -= Time.deltaTime;
		if (spawnTimer <= 0) {
			spawnTimer = spawnDelay;
			foreach (Spawnable spawnable in collection.spawnables){
				if (SpawnCheck(spawnable.initialSpawnProb,spawnable.failedSpawnCount)){
					Spawn(spawnable.name);
					Debug.Log(spawnable.name);
					spawnable.failedSpawnCount = 0;
				}
				else{
					spawnable.failedSpawnCount += 1;
					Debug.Log("FAIL!");
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
		Vector3 dir = player.GetComponent<Rigidbody>().velocity;
		Vector3 camPos = Camera.main.transform.position;
        Vector3 playerPos = player.GetComponent<Rigidbody>().position;

        Quaternion playerRot = player.GetComponent<Rigidbody>().rotation;
        Quaternion randomrot = Quaternion.Euler(0, Random.Range(-angleSpread / 2, angleSpread / 2), 0);
        
		float spawnRad = Random.Range(minRadius, minRadius+radiusSpread);
        Vector3 spawnPos = playerRot * randomrot * Vector3.forward * spawnRad;
		GameObject spawned = objectPooler.Spawn (name, spawnParent.transform, spawnPos + playerPos , Quaternion.identity);
	}
}
