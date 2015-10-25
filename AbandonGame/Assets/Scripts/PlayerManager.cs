using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour {
    public CarController car;
    public List<Material> carMaterials;
    [SerializeField] private Renderer carWonRenderer;

    private int numberOfPlayers;
    private int playersLeft;
    private int abandoningPlayerNum;
    private float[] xPositionOffset = new float[] { 0, -20, 20 };
    private float[] zPositionOffset = new float[] { -15, -10, -10 };
    private float[] rotationOffset = new float[] { 0, 22, -22 };

    void OnEnable()
    {
        Events.instance.AddListener<GameStarted>(OnGameStarted);
        Events.instance.AddListener<GameRestarted>(OnGameRestarted);
        Events.instance.AddListener<AbandonerChanged>(OnAbandonerChanged);
        Events.instance.AddListener<PlayerGotAbandoned>(OnPlayerGotAbandoned);
    }

    private void SpawnPlayers()
    {
        int abandoningPlayer = Random.Range(0, numberOfPlayers);
        for(int i = 0; i < numberOfPlayers; i++)
        {
            Vector3 carPos = Camera.main.transform.position;
            carPos.y -= 34.6f;
            carPos.z += 20;
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
            Renderer r = cc.transform.Find("Visual").Find("MuscleCar").Find("Body").GetComponent<MeshRenderer>();
            r.material = carMaterials[i];
            cc.playerNum = i + 1;

            cc.IsAbandoning = i == abandoningPlayer;
        }
    }

    private void StartGame()
    {
        numberOfPlayers = 0;
        foreach (string s in Input.GetJoystickNames())
        {
            if (s.Length > 0)
            {
                numberOfPlayers++;
            }
        }
        numberOfPlayers = Mathf.Clamp(numberOfPlayers, 1, 4);
        playersLeft = numberOfPlayers;
        SpawnPlayers();
    }

    private void OnGameStarted(GameStarted e)
    {
        StartGame();
    }

    private void OnGameRestarted(GameRestarted e)
    {
        StartGame();
    }

    private void OnAbandonerChanged(AbandonerChanged e)
    {
        abandoningPlayerNum = e.newAbandoner.GetComponent<CarController>().playerNum;
    }

    private void OnPlayerGotAbandoned(PlayerGotAbandoned e)
    {
        numberOfPlayers--;
        if(numberOfPlayers == 1)
        {
            carWonRenderer.material = carMaterials[abandoningPlayerNum-1];
            Events.instance.Raise(new PlayerWon(abandoningPlayerNum));
        }
    }

    void OnDisable()
    {
        Events.instance.RemoveListener<GameStarted>(OnGameStarted);
        Events.instance.RemoveListener<GameRestarted>(OnGameRestarted);
        Events.instance.RemoveListener<AbandonerChanged>(OnAbandonerChanged);
        Events.instance.RemoveListener<PlayerGotAbandoned>(OnPlayerGotAbandoned);
    }
}
