using UnityEngine;
using System.Collections;

public class SpawnableSuperclass : MonoBehaviour {
    public GameObject thisSpawnableObj;
    float despawnDist = 120f;
    protected GameObject baseObject;
    // Use this for initialization
    protected virtual void Start() {
        baseObject = gameObject;
    }
	
	// Update is called once per frame
	protected void Update ()
    {
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
        Events.instance.Raise(new ObjectDespawned(baseObject));
        Destroy(gameObject);
        //Reset();
    }
    protected virtual void Reset()
    {

    }

}
