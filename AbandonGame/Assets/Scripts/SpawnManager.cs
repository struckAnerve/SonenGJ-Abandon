using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour {

	
	public GameObject spawnParent;

	float spawnDelay;
	float spawnTimer;
	
	
	// Use this for initialization
	void Start () {
		spawnTimer = spawnDelay;
	}
	
	// Update is called once per frame
	void Update () {
		spawnTimer -= Time.deltaTime;
		
		if (spawnTimer <= 0) {
			Spawn();
		}
		
	}
	void Spawn(){
		
	}
}
