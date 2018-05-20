using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubePhysic : MonoBehaviour {

    public CubeState state;
    public int cubeNumber;
    private CubeController cubeController;
    private Renderer rend;
    public GameObject rocketShoe;
    public GameObject house;
    public GameObject rod;

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
            if(cubeNumber == 25)
            {
                Vector3 newPos = new Vector3(this.transform.position.x+0.6f, this.transform.position.y + 0.5f, this.transform.position.z-0.4f);
                Instantiate(house, newPos, Quaternion.Euler(new Vector3(0.0f,90.0f,0.0f)));
            }
        }
        else if (state == CubeState.CubeNormal)
        {
            rend.material.color = Color.green;
        }
        else if (state == CubeState.CubeHelp)
        {
            rend.material.color = Color.white;
            Vector3 newPos = new Vector3(this.transform.position.x, this.transform.position.y+0.5f,this.transform.position.z);
            Instantiate(rocketShoe,newPos,Quaternion.identity);
        }
        else if (state == CubeState.CubeDanger)
        {
            rend.material.color = Color.red;
            Vector3 newPos = new Vector3(this.transform.position.x, this.transform.position.y + 1f, this.transform.position.z);
            Instantiate(rod, newPos, Quaternion.identity);
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
