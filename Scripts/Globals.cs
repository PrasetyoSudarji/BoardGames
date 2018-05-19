using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Globals : NetworkBehaviour{
    [SyncVar]
    public int totalPlayer = 0;
    [SyncVar]
    public int maxPlayer = 4;
    [SyncVar]
    public bool isPlaying = false;
    [SyncVar]
    public int goals = 24;
    [SyncVar]
    public int playerTurn = 0;
    [SyncVar]
    public bool isMoving = false;
    [SyncVar]
    public bool isBackward = false;

    public override void OnStartServer()
    {
        totalPlayer = 0;
        isPlaying = false;
        playerTurn = 0;
        isMoving = false;
        isBackward = false;
    }

    void Update()
    {
        //Debug.Log("Player turn : "+playerTurn);
        if (playerTurn > totalPlayer)
        {
            playerTurn = 1;
        }

    }

}
