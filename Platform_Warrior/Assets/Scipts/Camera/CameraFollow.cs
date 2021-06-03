using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    private const string PLAYER_TAG = "Player";

    private const float cameraOffsetX = 10f;
    private const float cameraOffsetY = 3f;
    private const float smoothRunSpeed = 6f;
    private const float smoothJumpSpeed = 4f;
    private const float jumpOffset = 6.5f;
    private const float cameraOffsetZ = .05f;
    private const float maxCameraShift = -45f;

    private float smoothJumpSpeedDelta;
    private float smoothRunSpeedDelta;

    private Vector3 tempCameraPosition;
    private Vector3 latestRotationOffet;
    private Vector3 playerPosition;

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

        // set camera position
        tempCameraPosition = transform.position;
        CameraFollowAxes();
        transform.position = tempCameraPosition;

        // set camera rotation
        playerMovement = playerObject.GetComponent<PlayerMovement>();
        Vector3 rotationAngles;

        if ( playerMovement.IsFalling ) {
            rotationAngles = Vector3.Lerp( transform.eulerAngles , new Vector3( jumpOffset , 0f ) , smoothJumpSpeedDelta );
            latestRotationOffet = rotationAngles;

        } else {
            rotationAngles = Vector3.Lerp( latestRotationOffet , new Vector3( 0f , 0f ) , smoothJumpSpeedDelta );
            latestRotationOffet = rotationAngles;
        }

        transform.eulerAngles = rotationAngles;
    }

    private void CameraFollowAxes() {
        
        // X Axis
        Vector3 desiredPosition = playerPosition;
        Vector3 cameraOffsetVectorX = new Vector3( cameraOffsetX , 0f , 0f );

        if ( playerMovement.RunningRight == true ) {
            desiredPosition += cameraOffsetVectorX;

        } else if ( playerMovement.RunningLeft == true ) {
            desiredPosition -= cameraOffsetVectorX;
        }

        tempCameraPosition.x = Vector3.Lerp( tempCameraPosition , desiredPosition , smoothRunSpeedDelta ).x;

        // Y Axis
        desiredPosition = playerPosition + new Vector3( 0f , cameraOffsetY , 0f );
        tempCameraPosition.y = Vector3.Lerp( tempCameraPosition , desiredPosition , smoothRunSpeedDelta ).y;

        // Z Axis
        if ( ( playerMovement.IsJumping || playerMovement.IsFalling ) && tempCameraPosition.z >= maxCameraShift ) {
            tempCameraPosition.z -= new Vector3( 0f , 0f , cameraOffsetZ ).z;

        } else if ( !( playerMovement.IsJumping || playerMovement.IsFalling ) && tempCameraPosition.z <= maxCameraShift ) {
            tempCameraPosition.z += new Vector3( 0f , 0f , cameraOffsetZ ).z;
        }
    }
}
