using UnityEngine;
using System.Collections;

public class CarController : MonoBehaviour {
    private bool isAbandoning;
    public bool IsAbandoning
    {
        get { return isAbandoning; }
        set
        {
            isAbandoning = value;
            if(isAbandoning)
            {
                Events.instance.Raise(new AbandonerChanged(gameObject));
            }
        }
    }
    public string prefix;
    public int motorMultiplier;
    public int steerAngle;
    public int breakForce;
    AudioSource engAudio;

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

        engAudio = GetComponent<AudioSource>();
	}
	
    void Update() {
        currentMotorTorque = Input.GetAxis(prefix + "_Forward") * motorMultiplier;
        currentMotorTorque -= Input.GetAxis(prefix + "_Reverse") * motorMultiplier;

        currentSteerAngle = Input.GetAxis(prefix + "_Steer");

        if (Input.GetButtonDown(prefix + "_Break"))
        {
            currentBreakForce = breakForce;

            WheelFrictionCurve frictionCurve = wheelColRL.sidewaysFriction;
            frictionCurve.stiffness = 3.5f;
            wheelColRL.sidewaysFriction = frictionCurve;
            frictionCurve = wheelColRR.sidewaysFriction;
            frictionCurve.stiffness = 3.5f;
            wheelColRR.sidewaysFriction = frictionCurve;
        }
        else if (Input.GetButtonUp(prefix + "_Break"))
        {
            currentBreakForce = 0;

            WheelFrictionCurve frictionCurve = wheelColRL.sidewaysFriction;
            frictionCurve.stiffness = initialRearSidewaysFrictionStiffness;
            wheelColRL.sidewaysFriction = frictionCurve;
            frictionCurve = wheelColRR.sidewaysFriction;
            frictionCurve.stiffness = initialRearSidewaysFrictionStiffness;
            wheelColRR.sidewaysFriction = frictionCurve;
        }

        engAudio.pitch = rigidB.velocity.magnitude / 39 + (float)0.6;
    }

    void FixedUpdate () {
        wheelColRL.motorTorque = currentMotorTorque;
        wheelColRR.motorTorque = currentMotorTorque;

        wheelColRL.brakeTorque = currentBreakForce;
        wheelColRR.brakeTorque = currentBreakForce;

        wheelColFL.steerAngle = currentSteerAngle * steerAngle;
        wheelColFR.steerAngle = currentSteerAngle * steerAngle;
        
        rigidB.AddForce(Vector3.down * 100 * Vector3.Magnitude(rigidB.velocity));
    }
}
