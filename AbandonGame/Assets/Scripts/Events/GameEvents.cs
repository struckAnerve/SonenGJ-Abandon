using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class GameEvent 
{ }

public class GameStarted : GameEvent
{ }

public class GameRestarted : GameEvent
{ }

public class PlayerWon : GameEvent
{
    public int playerNum;

    public PlayerWon(int playerNum)
    {
        this.playerNum = playerNum;
    }
}

public class PlayerGotAbandoned : GameEvent
{
    public int playerNum;

    public PlayerGotAbandoned(int playerNum)
    {
        this.playerNum = playerNum;
    }
}

public class AbandonerChanged : GameEvent
{
    public GameObject newAbandoner;

    public AbandonerChanged(GameObject newAbandoner)
    {
        this.newAbandoner = newAbandoner;
    }
}
public class ObjectDespawned : GameEvent
{
    public GameObject objectToDespawn;
    public ObjectDespawned(GameObject objectToDespawn)
    {
        this.objectToDespawn = objectToDespawn;
    }
}