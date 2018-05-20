using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {
    public int playerId;
    public float speed;
    public int nextPosition;
    public int targetPosition;

    public bool myTurn = false;
    public Globals global;

    private DiceController diceController;
    private static GameObject boards;
    public GameObject winPanel;
    public GameObject losePanel;

    public int cubeSize;
    public static GameObject[] cube;
    public CameraFollowPlayer cam;

    public enum PlayerState
    {
        FacingRight,
        FacingLeft,
        FacingUp,
        FacingBottom
    }

    public PlayerState state;

    // Use this for initialization
    void Awake () {

        diceController = GameObject.FindGameObjectWithTag("Dice").GetComponent<DiceController>();
        if (diceController == null)
        {
            Debug.Log("Error Get DiceController");
        }
        else
        {
            Debug.Log("DiceController Loaded!!");
        }

        global = FindObjectOfType<Globals>();


    }

    public static void SetCube(GameObject[] newCube)
    {
        cube = new GameObject[newCube.Length];
        for(int i = 0; i < cube.Length; i++)
        {
            cube[i] = newCube[i];
        }
        
        if (cube != null)
        {
            Debug.Log(" New boards : " + cube.Length);
        }
    }

    private void Start()
    {
        if (isServer)
        {
            playerId = 1;
        }

        nextPosition = 0;
        targetPosition = 0;
        global.isMoving = false;
        state = PlayerState.FacingRight;
        Debug.Log("First state : " + state);
        global.isBackward = false;

    }

    // Update is called once per frame
    void Update () {

        if(hasAuthority == false)
        {
            return; 
        }

        if(cam == null)
        {
            cam = FindObjectOfType<CameraFollowPlayer>();
            cam.playerTransform = this.gameObject.transform;
            cam.global = global;
        }
        
        if(global.playerTurn == this.playerId)
        {
            myTurn = true;
            if (!global.isMoving && !global.isBackward && !diceController.isShake)
            {
                diceController.TurnOnShakeButton();
            }
            else
            {
                diceController.TurnOffShakeButton();
            }
        }
        else
        {
            myTurn = false;
            diceController.TurnOffShakeButton();
        }

        if (global.isWin)
        {
            Debug.Log("Player turn : "+global.playerTurn+",Player ID : " +this.playerId);
            if (global.playerTurn == this.playerId)
            {
                GetComponent<PlayerAnimationController>().Win();
                if (!global.isFinish)
                {
                    //Instantiate(winPanel);
                    GameObject go = GameObject.FindGameObjectWithTag("WinPanel");
                    for(int i = 0; i < go.transform.childCount; i++)
                    {
                        go.transform.GetChild(i).gameObject.SetActive(true);
                    }
                    global.isFinish = true;
                }
            }
            else
            {
                GetComponent<PlayerAnimationController>().Lose();
                if (!global.isFinish)
                {
                    //Instantiate(losePanel);
                    GameObject go = GameObject.FindGameObjectWithTag("LosePanel");
                    for (int i = 0; i < go.transform.childCount; i++)
                    {
                        go.transform.GetChild(i).gameObject.SetActive(true);
                    }
                    global.isFinish = true;
                }
            }

            TurnOff();
        }

        //movePlayer();
    }

    public void movePlayer()
    {
        if (myTurn)
        {
            if (global.isPlaying)
            {

                if (!global.isMoving && !global.isBackward)
                {
                    //diceController.ShakeDice();
                    targetPosition += diceController.diceNumber;
                    
                    StartMove(2.0f);
                }
            }
        }
    }

    public void StartMove(float seconds = 2.0f)
    {
        StartCoroutine("StartMoveCoroutine", seconds);
    }

    private IEnumerator StartMoveCoroutine(float seconds)
    {
        global.isMoving = true;
        GetComponent<PlayerAnimationController>().Walking();

        while (nextPosition != targetPosition)
        {
            if (nextPosition < targetPosition)
            {
                nextPosition += 1;
            }
            else if (nextPosition > targetPosition)
            {
                nextPosition -= 1;
            }

            Vector3 nextPos = new Vector3(cube[nextPosition].transform.position.x, this.transform.position.y, cube[nextPosition].transform.position.z);

            if (!global.isBackward)
            {
                if (this.transform.position.z < nextPos.z && state == PlayerState.FacingRight)
                {
                    state = PlayerState.FacingUp;
                    this.transform.rotation = Quaternion.Euler(this.transform.rotation.x, 0.0f, this.transform.rotation.z);
                    //Debug.Log("Player : " + player.transform.position.z + "target : " + targetPos.z);
                    //Debug.Log("After rotate from right state : " + state);
                }
                else if (this.transform.position.x > nextPos.x && state == PlayerState.FacingUp)
                {
                    state = PlayerState.FacingLeft;
                    this.transform.rotation = Quaternion.Euler(this.transform.rotation.x, -90.0f, this.transform.rotation.z);
                    //Debug.Log("Player : " + player.transform.position.x + "target : " + targetPos.x);
                    //Debug.Log("After rotate from Up state : " + state);
                }
                else if (this.transform.position.z < nextPos.z && state == PlayerState.FacingLeft)
                {
                    state = PlayerState.FacingUp;
                    this.transform.rotation = Quaternion.Euler(this.transform.rotation.x, 0.0f, this.transform.rotation.z);
                    //Debug.Log("Player : " + player.transform.position.z + "target : " + targetPos.z);
                    //Debug.Log("After rotate from Left state : " + state);
                }
                else if (this.transform.position.x < nextPos.x && state == PlayerState.FacingUp)
                {
                    state = PlayerState.FacingRight;
                    this.transform.rotation = Quaternion.Euler(this.transform.rotation.x, 90.0f, this.transform.rotation.z);
                    //Debug.Log("Player : " + player.transform.position.x + "target : " + targetPos.x);
                    //Debug.Log("After rotate from Up state : " + state);
                }
                else if (this.transform.position.x < nextPos.x && state == PlayerState.FacingLeft)
                {
                    state = PlayerState.FacingRight;
                    this.transform.rotation = Quaternion.Euler(this.transform.rotation.x, 90.0f, this.transform.rotation.z);
                    //Debug.Log("Player : " + player.transform.position.x + "target : " + targetPos.x);
                    //Debug.Log("After rotate from Left state : " + state);
                }
                else
                {
                    Debug.Log("No rotate : " + state);
                }
            }
            else
            {
                if (this.transform.position.z > nextPos.z && state == PlayerState.FacingLeft)
                {
                    state = PlayerState.FacingBottom;
                    this.transform.rotation = Quaternion.Euler(this.transform.rotation.x, 180.0f, this.transform.rotation.z);
                    //Debug.Log("Player : " + player.transform.position.z + "target : " + targetPos.z);
                    //Debug.Log("After rotate from left state : " + state);
                }
                else if (this.transform.position.x < nextPos.x && state == PlayerState.FacingBottom)
                {
                    state = PlayerState.FacingRight;
                    this.transform.rotation = Quaternion.Euler(this.transform.rotation.x, 90.0f, this.transform.rotation.z);
                    //Debug.Log("Player : " + player.transform.position.x + "target : " + targetPos.x);
                    //Debug.Log("After rotate from bottom state : " + state);
                }
                else if (this.transform.position.z < nextPos.z && state == PlayerState.FacingRight)
                {
                    state = PlayerState.FacingBottom;
                    this.transform.rotation = Quaternion.Euler(this.transform.rotation.x, 180.0f, this.transform.rotation.z);
                    //Debug.Log("Player : " + player.transform.position.z + "target : " + targetPos.z);
                    //Debug.Log("After rotate from Right state : " + state);
                }
                else if (this.transform.position.x > nextPos.x && state == PlayerState.FacingBottom)
                {
                    state = PlayerState.FacingLeft;
                    this.transform.rotation = Quaternion.Euler(this.transform.rotation.x, -90.0f, this.transform.rotation.z);
                    //Debug.Log("Player : " + player.transform.position.x + "target : " + targetPos.x);
                    //Debug.Log("After rotate from Up state : " + state);
                }
                else
                {
                    Debug.Log("No rotate : " + state);
                }
            }

            while (this.transform.position != nextPos)
            {
                float warning = Mathf.Min(seconds, seconds * 0.0125f);
                this.transform.position = Vector3.MoveTowards(this.transform.position, nextPos, speed * Time.deltaTime);
                yield return new WaitForSeconds(warning);
            }

            if(nextPosition == global.goals && targetPosition > global.goals)
            {
                global.isBackward = true;
                targetPosition = global.goals - (targetPosition - global.goals);
                state = PlayerState.FacingLeft;
                this.transform.rotation = Quaternion.Euler(this.transform.rotation.x, -90.0f, this.transform.rotation.z);
                //Debug.Log("After rotate from Right state : " + state);
            }else if(nextPosition == targetPosition && global.isBackward)
            {
                global.isBackward = false;
            }
        }

        checkPlayerPosition();

        this.GetComponent<PlayerAnimationController>().Idle();
        
        //global.isMoving = false;
    }

    public void checkPlayerPosition()
    {
        if (cube[targetPosition].gameObject.GetComponent<CubePhysic>().state == CubePhysic.CubeState.CubeDanger)
        {
            global.isBackward = true;
            //Debug.Log(cube[targetPosition].gameObject.GetComponent<CubePhysic>().state + "Move Backward : ");
            targetPosition -= Random.Range(1, 7);
            if (targetPosition < 0)
            {
                targetPosition = 0;
            }
            StartMove(2.0f);
            //global.isBackward = false;
        }
        else if (cube[targetPosition].gameObject.GetComponent<CubePhysic>().state == CubePhysic.CubeState.CubeHelp)
        {
            global.isBackward = false;
            //global.isMoving = true;
            //Debug.Log("Move Forward : " + state);
            targetPosition += Random.Range(1, 7);
            StartMove(2.0f);
        }
        else
        {
            CmdChangeTurn();
        }
    }

    [Command]
    void CmdChangeTurn()
    {
        if (targetPosition == global.goals)
        {
            RpcWin();
            Debug.Log("Win");
            global.isMoving = false;
            myTurn = false;
            
        }
        else
        {
            global.playerTurn += 1;
            global.isMoving = false;
            myTurn = false;
        }
        
    }

    [ClientRpc]
    void RpcWin()
    {
        global.isWin = true;
        global.isPlaying = false;
    }


    private void OnDestroy()
    {
        TurnOff();
    }

    void TurnOff()
    {
        if (diceController != null)
        {
            diceController.TurnOffAll();
        }
    }
}
