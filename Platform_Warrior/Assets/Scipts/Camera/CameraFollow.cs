using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    private const string PLAYER_TAG = "Player";

    private const float cameraOffsetX = 7f;
    private const float cameraOffsetY = 2f;
    private const float smoothSpeed = 5f;
    private float smoothSpeedDelta;

    private Vector3 tempCameraPosition;
    private Vector3 rotationAngles; 
    private Vector3 latestRotationOffet;
    private Vector3 cameraJumpRotationOffset;
    private Vector3 cameraNoRotation;
    private Vector3 cameraOffsetVectorX;
    private Vector3 cameraOffsetVectorY;
    private Vector3 desiredPosition; 
    private Vector3 playerPosition;

    private GameObject playerObject;
    private PlayerMovement playerMovement;

    // Start is called before the first frame update
    void Start() {
        playerObject = GameObject.FindWithTag(PLAYER_TAG);
        cameraOffsetVectorX = new Vector3(cameraOffsetX, 0f, 0f);
        cameraOffsetVectorY = new Vector3(0f, cameraOffsetY, 0f);
        cameraJumpRotationOffset = new Vector3(10f, 0f, 0f);
        cameraNoRotation = new Vector3(0f, 0f, 0f);
    }

    // Update is called once per frame
    void LateUpdate() {
        setCamera();
    }

    void setCamera() {
        // get necassary data 
        tempCameraPosition = transform.position;
        playerPosition = playerObject.transform.position;        
        playerMovement = playerObject.GetComponent<PlayerMovement>();
        rotationAngles = transform.eulerAngles;
        smoothSpeedDelta = smoothSpeed * Time.deltaTime;

        // modify temp data
        cameraFollowX(ref tempCameraPosition, playerPosition, playerMovement, smoothSpeedDelta);
        cameraFollowY(ref tempCameraPosition, playerPosition);
        cameraFollowJump(playerMovement, ref rotationAngles, smoothSpeedDelta);

        // apply the changes onto the camera
        transform.position = tempCameraPosition;
        transform.eulerAngles = rotationAngles;
    }

    private void cameraFollowJump(PlayerMovement playerMovement, ref Vector3 rotationAngles, float smoothSpeedDelta) {
        if( playerMovement.jumping ) {
            rotationAngles = Vector3.Lerp(transform.eulerAngles, cameraJumpRotationOffset, smoothSpeedDelta);
            latestRotationOffet = rotationAngles;
        } else {
            rotationAngles = Vector3.Lerp(latestRotationOffet, cameraNoRotation, smoothSpeedDelta);
            latestRotationOffet = rotationAngles;
        }
    }

    void cameraFollowY(ref Vector3 tempCameraPosition, Vector3 playerPosition) {
        tempCameraPosition = playerPosition + cameraOffsetVectorY;
    }

    void cameraFollowX(ref Vector3 tempCameraPosition, Vector3 playerPosition, PlayerMovement playerMovement, float smoothSpeedDelta) {
        // camera switching left or right from player according to direction player is running
        if( playerMovement.runningRight == true ) {
            desiredPosition = playerPosition + cameraOffsetVectorX;
        } else if( playerMovement.runningLeft == true ) {
            desiredPosition = playerPosition - cameraOffsetVectorX;
        }

        tempCameraPosition.x = Vector3.Lerp(tempCameraPosition, desiredPosition, smoothSpeedDelta).x;
    }
}
