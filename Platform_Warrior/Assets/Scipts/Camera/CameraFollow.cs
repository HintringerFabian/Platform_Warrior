using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    private const string PLAYER_TAG = "Player";

    private const float cameraOffsetX = 10f,
        cameraOffsetY = 3f,
        smoothRunSpeed = 7f,
        smoothJumpSpeed = 2.5f,
        jumpOffset = 6f;

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

        tempCameraPosition.x = Vector3.Lerp(tempCameraPosition, desiredPosition, smoothRunSpeed * Time.deltaTime).x;
    }
    private void cameraFollowY() {
        Vector3 desiredPosition = playerPosition + new Vector3(0f, cameraOffsetY, 0f);
        tempCameraPosition.y = Vector3.Lerp(tempCameraPosition, desiredPosition, smoothRunSpeed * Time.deltaTime).y;
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
        Vector3 cameraJumpRotationOffset = new Vector3(jumpOffset, 0f);
        Vector3 cameraNoRotation = new Vector3(0f, 0f);
        float smoothJumpSpeedDelta = smoothJumpSpeed * Time.deltaTime;

        if( playerMovement.isFalling ) {
            rotationAngles = Vector3.Lerp(transform.eulerAngles, cameraJumpRotationOffset, smoothJumpSpeedDelta);
            latestRotationOffet = rotationAngles;
        } else {
            rotationAngles = Vector3.Lerp(latestRotationOffet, cameraNoRotation, smoothJumpSpeedDelta);
            latestRotationOffet = rotationAngles;
        }

        transform.eulerAngles = rotationAngles;
    }
}
