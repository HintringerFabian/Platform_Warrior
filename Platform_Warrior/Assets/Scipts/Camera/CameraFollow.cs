using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    private const string PLAYER_TAG = "Player";
    private const float cameraOffsetY = 2f;
    private const float cameraOffsetX = 7f;
    private const float smoothSpeed = 5f;

    private Transform playerPosition;
    private Vector3 tempPos;
    Vector3 desiredPosition;

    GameObject playerObject;
    PlayerMovement playerMov;

    // Start is called before the first frame update
    void Start() {
        playerPosition = GameObject.FindWithTag(PLAYER_TAG).transform;
        playerObject = GameObject.FindWithTag(PLAYER_TAG);
    }

    // Update is called once per frame
    void LateUpdate() {
        setCameraPosition();
    }

    void setCameraPosition() {
        playerMov = playerObject.GetComponent<PlayerMovement>();

        tempPos = transform.position;

        // follow main character on Y axis
        tempPos.y = playerPosition.position.y + cameraOffsetY;

        
        if( playerMov.runningRight == true ) {
            desiredPosition = playerPosition.position + new Vector3(cameraOffsetX, 0f, 0f);
        } else if (playerMov.runningLeft == true ) {
            desiredPosition = playerPosition.position - new Vector3(cameraOffsetX, 0f, 0f);
        }

        Vector3 smoothenedPosition = Vector3.Lerp(tempPos, desiredPosition, smoothSpeed * Time.deltaTime);

        tempPos.x = smoothenedPosition.x;

        // apply the changes onto the camera
        transform.position = tempPos;
    }
}
