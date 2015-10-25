using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
    [SerializeField] private GameObject startMenu;
    [SerializeField] private GameObject restartMenu;
    [SerializeField] private List<GameObject> enableOnGameStart;
    private Text playerWonText;

    void OnEnable()
    {
        Events.instance.AddListener<PlayerWon>(OnPlayerWon);
    }

    void Start()
    {
        playerWonText = transform.Find("RestartMenu").Find("Headline").GetComponent<Text>();
        foreach (GameObject g in enableOnGameStart)
        {
            g.SetActive(false);
        }

        startMenu.SetActive(true);
        restartMenu.SetActive(false);
    }

    public void OnStartGameButtonPressed()
    {
        startMenu.SetActive(false);
        foreach (GameObject g in enableOnGameStart)
        {
            g.SetActive(true);
        }
        Events.instance.Raise(new GameStarted());
    }

    public void OnRestartGameButtonPressed()
    {
        restartMenu.SetActive(false);
        foreach (GameObject g in enableOnGameStart)
        {
            g.SetActive(true);
        }
        Events.instance.Raise(new GameRestarted());
    }

    private void OnPlayerWon(PlayerWon e)
    {
        playerWonText.text = "Player " + e.playerNum + " won!";
        restartMenu.SetActive(true);
    }

    void OnDisable()
    {
        Events.instance.RemoveListener<PlayerWon>(OnPlayerWon);
    }
}
