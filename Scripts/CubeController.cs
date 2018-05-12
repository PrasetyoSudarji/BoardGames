using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CubeController : NetworkBehaviour {

    public int totalCube;
    public GameObject[] cube;
    public PlayerConnectionObject.SyncListBoardState boardState;

    // Use this for initialization

    private void Awake()
    {
    }
    void Start () {

        cube = new GameObject[totalCube];

        for (int i = 0; i < totalCube; i++)
        {
            cube[i] = transform.GetChild(i).gameObject;
        }

        PlayerController.SetCube(this.cube);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

   

}
