using UnityEngine;
using System.Collections;

public class GameOverCar : MonoBehaviour {
    private Vector3 localOffset = new Vector3(13, 27, -10);
    public float rotationSpeed;

	// Update is called once per frame
	void Update () {
        transform.position = Camera.main.transform.position;
        transform.localPosition -= localOffset;
        transform.Rotate(0, Time.deltaTime * rotationSpeed, 0);
    }
}
