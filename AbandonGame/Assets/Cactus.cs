using UnityEngine;
using System.Collections;

public class Cactus : SpawnableSuperclass{ 
    public GameObject thisCactus;
    Rigidbody rb;
    float MaxWindForce = 7f;
	// Use this for initialization
	new void Start()
    {
        base.Start();
        rb = thisCactus.GetComponent<Rigidbody>();
        rb.centerOfMass = Vector3.down * 3.5f;
    }
    new void Update()
    {
        base.Update();
    }
	// Update is called once per frame
	void FixedUpdate ()
    {
        rb.AddTorque(Vector3.back*Random.Range(4,MaxWindForce));

	}
}
