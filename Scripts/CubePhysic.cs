using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubePhysic : MonoBehaviour {

    public CubeState state;
    public int cubeNumber;
    private CubeController cubeController;
    private Renderer rend;

    public enum CubeState
    {
        CubeDanger,
        CubeNormal,
        CubeHelp
    }

    private void Awake()
    {
        cubeController = gameObject.GetComponentInParent<CubeController>();
    }

    // Use this for initialization
    void Start () {
        //Get the renderer of the object so we can access the color
        rend = GetComponent<Renderer>();
        //Set the initial color (0f,0f,0f,0f)
        //Debug.Log(gameController.boardStates.GetItem(cubeNumber - 1).cubeNumber);
        //Debug.Log(gameController.boardStates.GetItem(cubeNumber - 1).state);

        getState();
        if(cubeNumber == 1 || cubeNumber == 25)
        {
            state = CubeState.CubeNormal;
            rend.material.color = Color.black;
        }
        else if (state == CubeState.CubeNormal)
        {
            rend.material.color = Color.green;
        }
        else if (state == CubeState.CubeHelp)
        {
            rend.material.color = Color.white;

        }
        else if (state == CubeState.CubeDanger)
        {
            rend.material.color = Color.red;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void getState()
    {
        state = cubeController.boardState.GetItem(cubeNumber - 1).state;
    }
}
