using UnityEngine;
using System.Collections;

public class SpawnableSuperclass : MonoBehaviour {
    public GameObject thisSpawnableObj;
    float despawnDist = 100f;
    // Use this for initialization
    protected void Start() {

    }
	
	// Update is called once per frame
	protected void Update ()
    {
        Debug.Log(CalculateCameraDist());
        if (CalculateCameraDist() > despawnDist)
        {
           Despawn();
        }
    }
    float CalculateCameraDist()
    {
        Vector3 camPos = Camera.main.transform.position;
        Vector3 spawnPos = thisSpawnableObj.transform.position;
        return Vector3.Distance(camPos, spawnPos);
    }
    void Despawn()
    {
        Events.instance.Raise(new ObjectDespawned());
        Object.Destroy(thisSpawnableObj);
    }

}
