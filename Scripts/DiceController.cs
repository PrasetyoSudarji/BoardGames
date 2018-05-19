using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DiceController : NetworkBehaviour {

    [SyncVar]
    public int diceNumber;

    public GameObject[] diceImg;
    public GameObject shakeButton;
    public Globals global;

    public bool isShake = false;

	// Use this for initialization
	void Start () {
        diceNumber = 0;
        global = FindObjectOfType<Globals>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!isLocalPlayer)
        {
            return;
        }

	}

    public void ShakeDice()
    {
        diceNumber = Random.Range(1, 7);
        Debug.Log("The dice is shaken");
        StartShake();
    }

    public void TurnOnShakeButton()
    {
        shakeButton.gameObject.SetActive(true);
    }

    public void TurnOffShakeButton()
    {
        shakeButton.gameObject.SetActive(false);
    }

    public void TurnOnDiceImg(int num)
    {
        diceImg[num-1].SetActive(true);
    }

    public void TurnOffDiceImg(int num)
    {
        diceImg[num-1].SetActive(false);
    }

    public void StartShake(float seconds = 2.0f)
    {
        StartCoroutine("StartShakeDice", seconds);
    }

    private IEnumerator StartShakeDice(float seconds)
    {
        isShake = true;
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        TurnOffShakeButton();

        TurnOnDiceImg(diceNumber);

        yield return new WaitForSeconds(seconds);

        TurnOffDiceImg(diceNumber);
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<PlayerController>().playerId == global.playerTurn)
            {
                players[i].GetComponent<PlayerController>().movePlayer();
            }
        }

        isShake = false;
    }
    
    public void TurnOffAll() {
        for(int i = 0; i < diceImg.Length; i++)
        {
            TurnOffDiceImg(i+1);
        }
        TurnOffShakeButton();
    }

}
