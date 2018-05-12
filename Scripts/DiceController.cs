using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DiceController : NetworkBehaviour {

    public int diceNumber;

	// Use this for initialization
	void Start () {
        diceNumber = 0;
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    public void shakeDice()
    {
        diceNumber = Random.Range(1, 7);
        Debug.Log("The dice is shaken");
    }
}
