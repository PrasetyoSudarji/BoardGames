﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class GameController : NetworkBehaviour {

    public Globals global;
    
    private void Awake()
    {
        global = FindObjectOfType<Globals>();
    }

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {

    }

    public void CloseGame()
    {
        SceneManager.LoadScene(1);
    }

}
