using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameController : NetworkBehaviour {

    public List<GameObject> playerList;
    public int playerTurn = 0;

    private void Awake()
    {
        
    }

    // Use this for initialization
    void Start () {
        playerList = new List<GameObject>(Globals.maxPlayer);
    }
	
	// Update is called once per frame
	void Update () {

        playerTurn = Globals.playerTurn;

        if(playerTurn > playerList.Count)
        {
            Globals.playerTurn = 1;
        }

        if (playerTurn == 1)
        {
            for(int i=0;i<playerList.Count;i++)
            {
                GameObject go = playerList[i];
                if(go.GetComponent<PlayerController>().playerId == 1 && !go.GetComponent<PlayerController>().myTurn)
                {
                    go.GetComponent<PlayerController>().myTurn = true;
                }
            }
        }
       
    }

    public void AddPlayer(GameObject player)
    {
        playerList.Add(player);
        Debug.Log("Player Added");
    }

}
