using UnityEngine;
using System.Collections;

public class CarController : MonoBehaviour {
    public int motorMultiplier;
    public int steerAngle;
    public int breakForce;

    private Rigidbody rigidB;
    [SerializeField] private WheelCollider wheelColFL;
    [SerializeField] private WheelCollider wheelColFR;
    [SerializeField] private WheelCollider wheelColRL;
    [SerializeField] private WheelCollider wheelColRR;
    private float currentMotorTorque;
    private float currentSteerAngle;

    // Use this for initialization
    void Start () {
        rigidB = gameObject.GetComponent<Rigidbody>();
        rigidB.centerOfMass = new Vector3(0, -0.5f, 0);
	}
	
	// Update is called once per frame
	void Update () {
        currentMotorTorque = Input.GetAxis("Forward") * motorMultiplier;
        currentMotorTorque -= Input.GetAxis("Reverse") * motorMultiplier;

        currentSteerAngle = Input.GetAxis("Steer");

        wheelColRL.motorTorque = currentMotorTorque;
        wheelColRR.motorTorque = currentMotorTorque;

        if(Input.GetButtonDown("Break"))
        {
            wheelColRL.brakeTorque = breakForce;
            wheelColRR.brakeTorque = breakForce;
        }
        else if(Input.GetButtonUp("Break"))
        {
            wheelColRL.brakeTorque = 0;
            wheelColRR.brakeTorque = 0;
        }

        wheelColFL.steerAngle = currentSteerAngle * steerAngle;
        wheelColFR.steerAngle = currentSteerAngle * steerAngle;
    }
}
