using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    private const string PLAYER_TAG = "Player";

    private const float cameraOffsetX = 10f,
        cameraOffsetY = 3f,
        smoothRunSpeed = 6f,
        smoothJumpSpeed = 4f,
        jumpOffset = 6.5f;

    private float smoothJumpSpeedDelta,
        smoothRunSpeedDelta;

    private Vector3 tempCameraPosition,
        latestRotationOffet,
        playerPosition;

    private GameObject playerObject;
    private PlayerMovement playerMovement;

    void Start() {
        playerObject = GameObject.FindWithTag(PLAYER_TAG);
    }

    private void LateUpdate() {
        playerMovement = playerObject.GetComponent<PlayerMovement>();
        playerPosition = playerObject.transform.position;

        smoothJumpSpeedDelta = smoothJumpSpeed * Time.deltaTime;
        smoothRunSpeedDelta = smoothRunSpeed * Time.deltaTime;

        setCameraPosition();
        setCameraAngle();
    }

    // set camera position
    private void setCameraPosition() {
        // get necassary data 
        tempCameraPosition = transform.position;

        // modify temp data
        cameraFollowX();
        cameraFollowY();
        cameraFollowZ();

        // apply the changes onto the camera
        transform.position = tempCameraPosition;
    }
    private void cameraFollowX() {
        Vector3 desiredPosition = playerPosition;
        Vector3 cameraOffsetVectorX = new Vector3(cameraOffsetX, 0f, 0f);
        // camera switching left or right from player according to direction player is running
        if( playerMovement.runningRight == true ) {
            desiredPosition += cameraOffsetVectorX;
        } else if( playerMovement.runningLeft == true ) {
            desiredPosition -= cameraOffsetVectorX;
        }

        tempCameraPosition.x = Vector3.Lerp(tempCameraPosition, desiredPosition, smoothRunSpeedDelta).x;
    }
    private void cameraFollowY() {
        Vector3 desiredPosition = playerPosition + new Vector3(0f, cameraOffsetY, 0f);
        tempCameraPosition.y = Vector3.Lerp(tempCameraPosition, desiredPosition, smoothRunSpeedDelta).y;
    }
    private void cameraFollowZ() {
        if( (playerMovement.isJumping || playerMovement.isFalling) && tempCameraPosition.z >=-50) {
            tempCameraPosition.z -= new Vector3(0f, 0f, .05f).z;
        } else if( (!playerMovement.isJumping || !playerMovement.isFalling) && tempCameraPosition.z <=-40 ) {
            tempCameraPosition.z += new Vector3(0f, 0f, .05f).z;
        }
    }

    // set camera angle
    private void setCameraAngle() {
        playerMovement = playerObject.GetComponent<PlayerMovement>();
        Vector3 rotationAngles;

        if( playerMovement.isFalling ) {
            rotationAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(jumpOffset, 0f), smoothJumpSpeedDelta);
            latestRotationOffet = rotationAngles;
        } else {
            rotationAngles = Vector3.Lerp(latestRotationOffet, new Vector3(0f, 0f), smoothJumpSpeedDelta);
            latestRotationOffet = rotationAngles;
        }

        transform.eulerAngles = rotationAngles;
    }
}
