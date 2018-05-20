using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerConnectionObject : NetworkBehaviour {

    public GameObject playerPrefab;
    public GameObject board;
    public CubeController cubeController;
    //private DiceController diceController;
    public Globals global;

    public struct BoardState
    {
        public int cubeNumber;
        public CubePhysic.CubeState state;
    }

    public class SyncListBoardState : SyncListStruct<BoardState> { }

    public SyncListBoardState boardStates = new SyncListBoardState();

    public void OnBoardStateChanged(SyncListBoardState.Operation op, int index)
    {
        //Debug.Log("list changed " + op);
    }

    public override void OnStartClient()
    {
        boardStates.Callback = OnBoardStateChanged;
    }

    private void Awake()
    {
        global = FindObjectOfType<Globals>();

    }

    // Use this for initialization
    void Start () {
        if (!isLocalPlayer)
        {
            return;
        }

        //diceController = GameObject.FindGameObjectWithTag("Dice").GetComponent<DiceController>();

        CmdSpawnMyPlayer(); 
        
	}
	
	// Update is called once per frame
	void Update () {
        if (!isLocalPlayer)
        {
            return;
        }

        if (!global.isPlaying)
        {
            if (isServer && global.totalPlayer >= 1 && Input.GetKeyDown(KeyCode.S))
            {
                CmdCreateBoards();

                global.isPlaying = true;

                global.playerTurn = 1;

                RpcAssignBoard();
            }
        }
    }

    [ClientRpc]
    void RpcInitiatePlayerId()
    {
        global.totalPlayer += 1;
        Debug.Log("Total Player : " + global.totalPlayer);
        playerPrefab.GetComponent<PlayerController>().playerId = global.totalPlayer;
    }

    [Command]
    void CmdSpawnMyPlayer()
    {
        
        RpcInitiatePlayerId();
        
        GameObject go = Instantiate(playerPrefab);

        NetworkServer.SpawnWithClientAuthority(go,connectionToClient);
        
    }

    [Command]
    void CmdCreateBoards()
    {
        GameObject go = Instantiate(board);

        NetworkServer.SpawnWithClientAuthority(go, connectionToClient);

        CmdCreateState();

        cubeController = GameObject.FindObjectOfType<CubeController>();
        cubeController.boardState = this.boardStates;
    }

    [Command]
    public void CmdCreateState()
    {
        int totalCube = board.transform.childCount;
        Debug.Log(totalCube);
        for (int i = 0; i < totalCube; i++)
        {
            BoardState newState = new BoardState();
            int perCent = Random.Range(0, 100);
            if (i == 0 || i == 24)
            {

                newState.cubeNumber = i;
                newState.state = CubePhysic.CubeState.CubeNormal;

                boardStates.Add(newState);
            }
            else if (perCent < 80)
            {
                newState.cubeNumber = i;
                newState.state = CubePhysic.CubeState.CubeNormal;

                boardStates.Add(newState);
            }
            else if (perCent < 80 + 10)
            {
                newState.cubeNumber = i;
                newState.state = CubePhysic.CubeState.CubeHelp;

                boardStates.Add(newState);
            }
            else if (perCent < 80 + 10 + 10)
            {
                newState.cubeNumber = i;
                newState.state = CubePhysic.CubeState.CubeDanger;

                boardStates.Add(newState);
            }
        }
    }

    [ClientRpc]
    public void RpcAssignBoard()
    {
        cubeController = GameObject.FindObjectOfType<CubeController>();
        cubeController.boardState = this.boardStates;
    }

}
