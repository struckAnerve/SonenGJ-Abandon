using UnityEngine;
using System.Collections;

public class Cactus : SpawnableSuperclass
{
    public GameObject thisCactus;
    Rigidbody rb;
    float MaxWindForce = 10f;
    // Use this for initialization
    new void Start()
    {
        base.Start();
        rb = thisCactus.GetComponentInChildren<Rigidbody>();
        rb.centerOfMass = Vector3.down * 2.5f;

        rb.constraints = RigidbodyConstraints.FreezePosition;
    }
    new void Update()
    {
        base.Update();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        rb.AddTorque(Vector3.back * Random.Range(4, MaxWindForce));
    }
    void OnCollisionEnter(Collision col)
    {
        CarController cc = col.gameObject.GetComponent<CarController>();

        if (cc != null)
        {

            Vector3 colVel = col.rigidbody.velocity;
            Vector3 dampForce = colVel.normalized * -300000;
            float colSpeed = colVel.magnitude;
            if (colSpeed < 10)
            {
                rb.constraints = RigidbodyConstraints.FreezePosition;
            }
            else
            {
                rb.constraints = RigidbodyConstraints.None;
                rb.AddExplosionForce(colSpeed * 25, col.transform.position + Vector3.up * 4, 10, 0.3f);

                rb.centerOfMass = Vector3.zero;
                col.rigidbody.AddForce(dampForce);

                //rb.AddTorque(Vector3.forward * 2);
            }
        }
    }
    void OnCollisionExit(Collision col)
    {

        rb.constraints = RigidbodyConstraints.None;

    }
}