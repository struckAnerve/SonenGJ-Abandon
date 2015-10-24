using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class GameEvent 
{ }

public class AbandonerChanged : GameEvent
{
    public GameObject newAbandoner;

    public AbandonerChanged(GameObject newAbandoner)
    {
        this.newAbandoner = newAbandoner;
    }
}