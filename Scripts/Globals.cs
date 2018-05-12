using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Globals {
    public static int totalPlayer = 0;
    public static int maxPlayer = 4;
    public static bool isPlaying = false;
    public static int goals = 24;
    public static int playerTurn = 0;
    [SyncVar]
    public static bool isMoving = false;
    [SyncVar]
    public static bool isBackward = false;

}
