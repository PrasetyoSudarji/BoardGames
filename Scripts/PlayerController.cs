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

    private DiceController diceController;
    private static GameObject boards;

    public int cubeSize;
    public static GameObject[] cube;


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
        nextPosition = 0;
        targetPosition = 0;
        Globals.isMoving = false;
        state = PlayerState.FacingRight;
        Debug.Log("First state : " + state);
        Globals.isBackward = false;
    }

    // Update is called once per frame
    void Update () {

        if(hasAuthority == false)
        {
            return; 
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            this.transform.Translate(0, 1, 0);
        }

        if (myTurn)
        {
            if (Globals.isPlaying)
            {

                if (!Globals.isMoving && !Globals.isBackward)
                {
                    if (Input.GetKeyDown(KeyCode.R))
                    {
                        diceController.shakeDice();
                        targetPosition += diceController.diceNumber;
                        Globals.isMoving = true;
                        StartMove(2.0f);
                    }
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

            if (!Globals.isBackward)
            {
                if (this.transform.position.z < nextPos.z && state == PlayerState.FacingRight)
                {
                    state = PlayerState.FacingUp;
                    this.transform.rotation = Quaternion.Euler(this.transform.rotation.x, 0.0f, this.transform.rotation.z);
                    //Debug.Log("Player : " + player.transform.position.z + "target : " + targetPos.z);
                    Debug.Log("After rotate from right state : " + state);
                }
                else if (this.transform.position.x > nextPos.x && state == PlayerState.FacingUp)
                {
                    state = PlayerState.FacingLeft;
                    this.transform.rotation = Quaternion.Euler(this.transform.rotation.x, -90.0f, this.transform.rotation.z);
                    //Debug.Log("Player : " + player.transform.position.x + "target : " + targetPos.x);
                    Debug.Log("After rotate from Up state : " + state);
                }
                else if (this.transform.position.z < nextPos.z && state == PlayerState.FacingLeft)
                {
                    state = PlayerState.FacingUp;
                    this.transform.rotation = Quaternion.Euler(this.transform.rotation.x, 0.0f, this.transform.rotation.z);
                    //Debug.Log("Player : " + player.transform.position.z + "target : " + targetPos.z);
                    Debug.Log("After rotate from Left state : " + state);
                }
                else if (this.transform.position.x < nextPos.x && state == PlayerState.FacingUp)
                {
                    state = PlayerState.FacingRight;
                    this.transform.rotation = Quaternion.Euler(this.transform.rotation.x, 90.0f, this.transform.rotation.z);
                    //Debug.Log("Player : " + player.transform.position.x + "target : " + targetPos.x);
                    Debug.Log("After rotate from Up state : " + state);
                }
                else if (this.transform.position.x < nextPos.x && state == PlayerState.FacingLeft)
                {
                    state = PlayerState.FacingRight;
                    this.transform.rotation = Quaternion.Euler(this.transform.rotation.x, 90.0f, this.transform.rotation.z);
                    //Debug.Log("Player : " + player.transform.position.x + "target : " + targetPos.x);
                    Debug.Log("After rotate from Left state : " + state);
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
                    Debug.Log("After rotate from left state : " + state);
                }
                else if (this.transform.position.x < nextPos.x && state == PlayerState.FacingBottom)
                {
                    state = PlayerState.FacingRight;
                    this.transform.rotation = Quaternion.Euler(this.transform.rotation.x, 90.0f, this.transform.rotation.z);
                    //Debug.Log("Player : " + player.transform.position.x + "target : " + targetPos.x);
                    Debug.Log("After rotate from bottom state : " + state);
                }
                else if (this.transform.position.z < nextPos.z && state == PlayerState.FacingRight)
                {
                    state = PlayerState.FacingBottom;
                    this.transform.rotation = Quaternion.Euler(this.transform.rotation.x, 180.0f, this.transform.rotation.z);
                    //Debug.Log("Player : " + player.transform.position.z + "target : " + targetPos.z);
                    Debug.Log("After rotate from Right state : " + state);
                }
                else if (this.transform.position.x > nextPos.x && state == PlayerState.FacingBottom)
                {
                    state = PlayerState.FacingLeft;
                    this.transform.rotation = Quaternion.Euler(this.transform.rotation.x, -90.0f, this.transform.rotation.z);
                    //Debug.Log("Player : " + player.transform.position.x + "target : " + targetPos.x);
                    Debug.Log("After rotate from Up state : " + state);
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

            if(nextPosition == Globals.goals && targetPosition > Globals.goals)
            {
                Globals.isBackward = true;
                targetPosition = Globals.goals - (targetPosition - Globals.goals);
                state = PlayerState.FacingLeft;
                this.transform.rotation = Quaternion.Euler(this.transform.rotation.x, -90.0f, this.transform.rotation.z);
                Debug.Log("After rotate from Right state : " + state);
            }else if(nextPosition == targetPosition && Globals.isBackward)
            {
                Globals.isBackward = false;
            }
        }

        checkPlayerPosition();

        if (targetPosition == Globals.goals)
        {
            Debug.Log("Win");
        }

        this.GetComponent<PlayerAnimationController>().Idle();
        Globals.isMoving = false;
        Globals.playerTurn = 2;
        myTurn = false;
    }

    public void checkPlayerPosition()
    {
        if (cube[targetPosition].gameObject.GetComponent<CubePhysic>().state == CubePhysic.CubeState.CubeDanger)
        {
            Globals.isBackward = true;
            Debug.Log(cube[targetPosition].gameObject.GetComponent<CubePhysic>().state + "Move Backward : ");
            targetPosition -= Random.Range(1, 7);
            if (targetPosition < 0)
            {
                targetPosition = 0;
            }
            StartMove(2.0f);
        }
        else if (cube[targetPosition].gameObject.GetComponent<CubePhysic>().state == CubePhysic.CubeState.CubeHelp)
        {
            Globals.isMoving = true;
            Debug.Log("Move Forward : " + state);
            targetPosition += Random.Range(1, 7);
            StartMove(2.0f);
        }
    }


}
