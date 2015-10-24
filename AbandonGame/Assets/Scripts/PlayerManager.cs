using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {
    public CarController car;

    private int numberOfPlayers;
    private float[] xPositionOffset = new float[] { 0, -20, 20 };
    private float[] zPositionOffset = new float[] { -15, -10, -10 };
    private float[] rotationOffset = new float[] { 0, 22, -22 };

	void Start () {
        numberOfPlayers = Input.GetJoystickNames().Length;
        SpawnPlayers();
	}
	
	void Update () {
	
	}

    private void SpawnPlayers()
    {
        int abandoningPlayer = Random.Range(0, numberOfPlayers);
        for(int i = 0; i < numberOfPlayers; i++)
        {
            Vector3 carPos = car.transform.position;
            Vector3 carRot = car.transform.rotation.eulerAngles;
            if (i < abandoningPlayer)
            {
                carPos.x += xPositionOffset[i];
                carPos.z += zPositionOffset[i];
                carRot.y += rotationOffset[i];
            }
            else if(i > abandoningPlayer)
            {
                carPos.x += xPositionOffset[i - 1];
                carPos.z += zPositionOffset[i - 1];
                carRot.y += rotationOffset[i - 1];
            }
            CarController cc = Instantiate(car, carPos, Quaternion.Euler(carRot)) as CarController;
            cc.prefix = "P" + (i + 1);

            if(i == abandoningPlayer)
            {
                Events.instance.Raise(new AbandonerChanged(cc.gameObject));
            }
        }
    }
}
