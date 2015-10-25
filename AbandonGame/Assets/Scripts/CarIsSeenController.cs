using UnityEngine;
using System.Collections;

public class CarIsSeenController : MonoBehaviour {
    private float timeNotSeen;

    void Update()
    {
        if(timeNotSeen > 1.5f)
        {
            GameObject car = transform.root.gameObject;
            if (!car.GetComponent<CarController>().IsAbandoning)
            {
                Events.instance.Raise(new PlayerGotAbandoned(car.GetComponent<CarController>().playerNum));
                Destroy(car);
            }
        }
        timeNotSeen += Time.deltaTime;
    }

    void OnWillRenderObject()
    {
        timeNotSeen = 0;
    }
}
