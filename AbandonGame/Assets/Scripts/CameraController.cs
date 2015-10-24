using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
    public GameObject objectToFollow;

    private const float cameraZOffset = 20;
    private const float zMaxDistanceToPlayer = 20 + cameraZOffset;
    private const float zMinDistanceToPlayer = 10;
    private const float maxHorizontalDistanceToPlayer = 35;
    private Vector3 desiredPos;
    private Vector3 clampedPos;

    void Start()
    {
        desiredPos = transform.position;
    }
	
	void FixedUpdate () {
        //Slerpingly follow object 
        desiredPos.x = objectToFollow.transform.position.x;
        desiredPos.z = objectToFollow.transform.position.z - cameraZOffset;
        transform.position = Vector3.Slerp(transform.position, desiredPos, Time.fixedDeltaTime * Time.deltaTime);


        //Clamp position so as to not fall too fra behind
        clampedPos = transform.position;
        clampedPos.x = Mathf.Clamp(transform.position.x, objectToFollow.transform.position.x - maxHorizontalDistanceToPlayer, objectToFollow.transform.position.x + maxHorizontalDistanceToPlayer);
        clampedPos.z = Mathf.Clamp(transform.position.z, objectToFollow.transform.position.z - zMaxDistanceToPlayer, objectToFollow.transform.position.z - zMinDistanceToPlayer);
        transform.position = clampedPos;
	}
}
