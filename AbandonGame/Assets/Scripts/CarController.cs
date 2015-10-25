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
    public int playerNum;
    public int motorMultiplier;
    public int steerAngle;
    public int breakForce;

    AudioSource engAudio;
    public AudioClip crashSound1;
    public AudioClip crashSound2;
    public AudioSource honkSound;
    public AudioSource CrashSound;

    private Rigidbody rigidB;
    [SerializeField] private MeshRenderer bodyMesh;
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
    private bool lostControl;

    private float timeOutOfFrame;

    void OnEnable()
    {
        Events.instance.AddListener<PlayerWon>(OnPlayerWon);
        Events.instance.AddListener<GameRestarted>(OnGameRestarted);
    }

    void Start () {
        rigidB = gameObject.GetComponent<Rigidbody>();
        rigidB.centerOfMass = new Vector3(0, -0.5f, 0);

        initialRearSidewaysFrictionStiffness = wheelColRL.sidewaysFriction.stiffness;

        lostControl = false;

        engAudio = GetComponent<AudioSource>();
	}
	
    void Update() {
        startedAbandoningThisFrame = false;

        if (!lostControl)
        {
            currentMotorTorque = Input.GetAxis("P" + playerNum + "_Forward") * motorMultiplier;
            currentMotorTorque -= Input.GetAxis("P" + playerNum + "_Reverse") * motorMultiplier;

            currentSteerAngle = Input.GetAxis("P" + playerNum + "_Steer");

            if (Input.GetButtonDown("P" + playerNum + "_Break"))
            {
                currentBreakForce = breakForce;

                WheelFrictionCurve frictionCurve = wheelColRL.sidewaysFriction;
                frictionCurve.stiffness = 3f;
                wheelColRL.sidewaysFriction = frictionCurve;
                frictionCurve = wheelColRR.sidewaysFriction;
                frictionCurve.stiffness = 3f;
                wheelColRR.sidewaysFriction = frictionCurve;
            }
            else if (Input.GetButtonUp("P" + playerNum + "_Break"))
            {
                currentBreakForce = 0;

                WheelFrictionCurve frictionCurve = wheelColRL.sidewaysFriction;
                frictionCurve.stiffness = initialRearSidewaysFrictionStiffness;
                wheelColRL.sidewaysFriction = frictionCurve;
                frictionCurve = wheelColRR.sidewaysFriction;
                frictionCurve.stiffness = initialRearSidewaysFrictionStiffness;
                wheelColRR.sidewaysFriction = frictionCurve;
            }
            if (Input.GetButtonDown("P" + playerNum + "_Honk")) {
                honkSound.Play();
            }
        }
        else
        {
            currentMotorTorque = 0;
            currentSteerAngle = 0;
            currentBreakForce = 0;
        }

        wheelSteeringAngle.y = wheelColFL.steerAngle;
        wheelFL.transform.localRotation = Quaternion.Euler(wheelSteeringAngle);
        wheelSteeringAngle.y = wheelColFR.steerAngle;
        wheelFR.transform.localRotation = Quaternion.Euler(wheelSteeringAngle);

        wheelFL.transform.Rotate(wheelColFL.rpm * 60 * Time.deltaTime, 0, 0);
        wheelFR.transform.Rotate(wheelColFR.rpm * 60 * Time.deltaTime, 0, 0);
        wheelRL.transform.Rotate(wheelColRL.rpm * 60 * Time.deltaTime, 0, 0);
        wheelRR.transform.Rotate(wheelColRR.rpm * 60 * Time.deltaTime, 0, 0);

        engAudio.pitch = rigidB.velocity.magnitude / 32 + 0.6f;
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
                
                CrashSound.Play();
                Debug.Log("Krash");
            }
        }
        if(!IsAbandoning && col.gameObject.tag == "Player") {
            SoundManager.instance.RandomizeSFX(crashSound1, crashSound2);
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

    private void OnPlayerWon(PlayerWon e)
    {
        lostControl = true;
    }

    private void OnGameRestarted(GameRestarted e)
    {
        Destroy(gameObject);
    }

    void OnDisable()
    {
        Events.instance.RemoveListener<PlayerWon>(OnPlayerWon);
        Events.instance.RemoveListener<GameRestarted>(OnGameRestarted);
    }
}
