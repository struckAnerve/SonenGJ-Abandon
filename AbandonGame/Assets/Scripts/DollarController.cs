using UnityEngine;
using System.Collections;

public class DollarController : MonoBehaviour {
    public float rotationSpeed;
    public Vector3 offset;
    private GameObject abandoningPlayer;

	
    void OnEnable()
    {
        Events.instance.AddListener<AbandonerChanged>(OnAbandonerChanged);
    }

	// Update is called once per frame
	void Update () {
        transform.Rotate(0, Time.deltaTime * rotationSpeed, 0);
        transform.position = abandoningPlayer.transform.position + offset;
	}

    private void OnAbandonerChanged(AbandonerChanged e)
    {
        abandoningPlayer = e.newAbandoner;
    }

    void OnDisable()
    {
        Events.instance.RemoveListener<AbandonerChanged>(OnAbandonerChanged);
    }
}
