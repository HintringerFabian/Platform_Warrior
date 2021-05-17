using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    private const string PLAYER_TAG = "Player";

    private const float cameraOffsetX = 7f,
        cameraOffsetY = 2f,
        smoothSpeed = 7f,
        smoothJumpSpeed = 4f,
        jumpOffset = 5f;

    private float smoothSpeedDelta,
        smoothJumpSpeedDelta;

    private Vector3 tempCameraPosition,
        rotationAngles,
        latestRotationOffet,
        cameraJumpRotationOffset,
        cameraNoRotation,
        cameraOffsetVectorX,
        cameraOffsetVectorY,
        desiredPosition,
        playerPosition;

    private GameObject playerObject;
    private PlayerMovement playerMovement;

    void Start() {
        playerObject = GameObject.FindWithTag(PLAYER_TAG);
        cameraOffsetVectorX = new Vector3(cameraOffsetX, 0f, 0f);
        cameraOffsetVectorY = new Vector3(0f, cameraOffsetY, 0f);
        cameraJumpRotationOffset = new Vector3(jumpOffset, 0f, 0f);
        cameraNoRotation = new Vector3(0f, 0f, 0f);
    }

    private void LateUpdate() {
        setCamera();
    }

    private void FixedUpdate() {
        setCameraAngle();
    }

    private void setCamera() {
        // get necassary data 
        tempCameraPosition = transform.position;
        playerPosition = playerObject.transform.position;
        playerMovement = playerObject.GetComponent<PlayerMovement>();
        smoothSpeedDelta = smoothSpeed * Time.deltaTime;


        // modify temp data
        cameraFollowX();
        cameraFollowY();

        // apply the changes onto the camera
        transform.position = tempCameraPosition;

    }

    private void setCameraAngle() {
        playerMovement = playerObject.GetComponent<PlayerMovement>();
        rotationAngles = transform.eulerAngles;
        smoothJumpSpeedDelta = smoothJumpSpeed * Time.deltaTime;

        cameraFollowJump();

        transform.eulerAngles = rotationAngles;
    }

    private void cameraFollowJump() {
        if( playerMovement.jumping ) {
            rotationAngles = Vector3.Lerp(transform.eulerAngles, cameraJumpRotationOffset, smoothJumpSpeedDelta);
            latestRotationOffet = rotationAngles;
        } else {
            rotationAngles = Vector3.Lerp(latestRotationOffet, cameraNoRotation, smoothJumpSpeedDelta);
            latestRotationOffet = rotationAngles;
        }
    }

    private void cameraFollowY() {
        tempCameraPosition.y = (playerPosition + cameraOffsetVectorY).y;
    }

    private void cameraFollowX() {
        // camera switching left or right from player according to direction player is running
        if( playerMovement.runningRight == true ) {
            desiredPosition = playerPosition + cameraOffsetVectorX;
        } else if( playerMovement.runningLeft == true ) {
            desiredPosition = playerPosition - cameraOffsetVectorX;
        }

        tempCameraPosition.x = Vector3.Lerp(tempCameraPosition, desiredPosition, smoothSpeedDelta).x;
    }
}
