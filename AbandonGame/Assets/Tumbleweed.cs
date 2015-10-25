using UnityEngine;
using System.Collections;

public class Tumbleweed : SpawnableSuperclass {

    // Use this for initialization

    Rigidbody rb;

    float MaxWindForce = 7f;
    new void Start()
    {
        base.Start();
        rb = thisSpawnableObj.GetComponentInChildren<Rigidbody>();
    }
    new void Update()
    {
        base.Update();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.AddTorque(Vector3.back * Random.Range(5, MaxWindForce));
    }
}
