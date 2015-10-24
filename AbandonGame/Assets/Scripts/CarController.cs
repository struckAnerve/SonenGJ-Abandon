using UnityEngine;
using System.Collections;

public class CarController : MonoBehaviour {
    private bool startedAbandoningThisFrame;
    private bool isAbandoning;
    public bool IsAbandoning
    {
        get { return isAbandoning; }
        set
        {
            isAbandoning = value;
            if(isAbandoning)
            {
                startedAbandoningThisFrame = true;
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
    [SerializeField] private GameObject wheelFL;
    [SerializeField] private GameObject wheelFR;
    [SerializeField] private GameObject wheelRL;
    [SerializeField] private GameObject wheelRR;
    private Vector3 wheelSteeringAngle = Vector3.zero;
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
        startedAbandoningThisFrame = false;

        currentMotorTorque = Input.GetAxis(prefix + "_Forward") * motorMultiplier;
        currentMotorTorque -= Input.GetAxis(prefix + "_Reverse") * motorMultiplier;

        currentSteerAngle = Input.GetAxis(prefix + "_Steer");

        if (Input.GetButtonDown(prefix + "_Break"))
        {
            currentBreakForce = breakForce;

            WheelFrictionCurve frictionCurve = wheelColRL.sidewaysFriction;
            frictionCurve.stiffness = 3f;
            wheelColRL.sidewaysFriction = frictionCurve;
            frictionCurve = wheelColRR.sidewaysFriction;
            frictionCurve.stiffness = 3f;
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

        wheelSteeringAngle.y = wheelColFL.steerAngle;
        wheelFL.transform.localRotation = Quaternion.Euler(wheelSteeringAngle);
        wheelSteeringAngle.y = wheelColFR.steerAngle;
        wheelFR.transform.localRotation = Quaternion.Euler(wheelSteeringAngle);

        wheelFL.transform.Rotate(wheelColFL.rpm * 60 * Time.deltaTime, 0, 0);
        wheelFR.transform.Rotate(wheelColFR.rpm * 60 * Time.deltaTime, 0, 0);
        wheelRL.transform.Rotate(wheelColRL.rpm * 60 * Time.deltaTime, 0, 0);
        wheelRR.transform.Rotate(wheelColRR.rpm * 60 * Time.deltaTime, 0, 0);

        

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

    public void CrashForce(Vector3 pos)
    {
        rigidB.AddExplosionForce(500000, pos, 10, 0.075f);
    }

    void OnCollisionEnter(Collision col)
    {
        if(IsAbandoning && !startedAbandoningThisFrame)
        {
            CarController colCar = col.gameObject.GetComponentInParent<CarController>();
            if(colCar != null)
            { 
                IsAbandoning = false;
                colCar.IsAbandoning = true;

                CrashForce(col.contacts[0].point);
                colCar.CrashForce(col.contacts[0].point);
            }
        }
    }

    void OnCollisionStay(Collision col)
    {
        if(col.gameObject.GetComponent<TerrainCollider>() != null)
        {
            if(transform.up.y < -0.9f)
            {
                rigidB.AddTorque(transform.forward * 10000);
            }
        }
    }
}
