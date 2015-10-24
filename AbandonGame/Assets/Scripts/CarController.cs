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
    private float initialRearSidewaysFrictionStiffness;
    private int currentBreakForce;

    // Use this for initialization
    void Start () {
        rigidB = gameObject.GetComponent<Rigidbody>();
        rigidB.centerOfMass = new Vector3(0, -0.5f, 0);

        initialRearSidewaysFrictionStiffness = wheelColRL.sidewaysFriction.stiffness;
	}
	
    void Update() {
        currentMotorTorque = Input.GetAxis("Forward") * motorMultiplier;
        currentMotorTorque -= Input.GetAxis("Reverse") * motorMultiplier;

        currentSteerAngle = Input.GetAxis("Steer");

        if (Input.GetButtonDown("Break"))
        {
            currentBreakForce = breakForce;

            WheelFrictionCurve frictionCurve = wheelColRL.sidewaysFriction;
            frictionCurve.stiffness = 3.5f;
            wheelColRL.sidewaysFriction = frictionCurve;
            frictionCurve = wheelColRR.sidewaysFriction;
            frictionCurve.stiffness = 3.5f;
            wheelColRR.sidewaysFriction = frictionCurve;
        }
        else if (Input.GetButtonUp("Break"))
        {
            currentBreakForce = 0;

            WheelFrictionCurve frictionCurve = wheelColRL.sidewaysFriction;
            frictionCurve.stiffness = initialRearSidewaysFrictionStiffness;
            wheelColRL.sidewaysFriction = frictionCurve;
            frictionCurve = wheelColRR.sidewaysFriction;
            frictionCurve.stiffness = initialRearSidewaysFrictionStiffness;
            wheelColRR.sidewaysFriction = frictionCurve;
        }
    }

    void FixedUpdate () {
        wheelColRL.motorTorque = currentMotorTorque;
        wheelColRR.motorTorque = currentMotorTorque;

        wheelColRL.brakeTorque = currentBreakForce;
        wheelColRR.brakeTorque = currentBreakForce;

        wheelColFL.steerAngle = currentSteerAngle * steerAngle;
        wheelColFR.steerAngle = currentSteerAngle * steerAngle;

        Debug.Log(rigidB.velocity.magnitude * 60 * 60 / 1000);
        rigidB.AddForce(Vector3.down * 100 * Vector3.Magnitude(rigidB.velocity));
    }
}
