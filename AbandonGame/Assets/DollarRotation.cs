using UnityEngine;
using System.Collections;

public class DollarRotation : MonoBehaviour {
    public float rotationSpeed;
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(0, Time.deltaTime * rotationSpeed, 0);
	}
}
