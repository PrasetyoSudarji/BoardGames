using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour {
    private Transform playerTransform;

    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    // Update is called once per frame

    public void Start()
    {
        
    }

    void LateUpdate () {
        if (playerTransform != null)
        {
            Vector3 desiredPosition = playerTransform.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            transform.position = smoothedPosition;

            transform.LookAt(playerTransform);
        }
	}

    public void FocusOn(int playerNumber)
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }
}
