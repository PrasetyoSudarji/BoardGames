using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour {
    public Transform playerTransform;

    public float smoothSpeed = 0.125f;
    public Vector3 offset;
    public Globals global;

    // Update is called once per frame

    public void Start()
    {
        
    }

    void Update () {

        if (playerTransform != null && global.isPlaying)
        {
            //Debug.Log("Camera Follow the player");
            Vector3 desiredPosition = playerTransform.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            transform.position = smoothedPosition;

            transform.LookAt(playerTransform);
        }
    }

    public void FocusOn(Transform playerTrans)
    {
        playerTransform = playerTrans;
    }
}
