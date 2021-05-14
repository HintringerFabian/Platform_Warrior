using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    private const string PLAYER_TAG = "Player";

    private const float cameraOffsetY = 2f;
    private const float cameraOffsetX = 7f;
    private const float smoothSpeed = 5f;

    private Vector3 tempPos;
    private Vector3 desiredPosition;
    private Vector3 cameraOffsetVectorX;
    private Vector3 playerPosition;

    private GameObject playerObject;
    private PlayerMovement playerMov;

    // Start is called before the first frame update
    void Start() {
        playerObject = GameObject.FindWithTag(PLAYER_TAG);
        cameraOffsetVectorX = new Vector3(cameraOffsetX, 0f, 0f);
    }

    // Update is called once per frame
    void LateUpdate() {
        setCameraPosition();
    }

    void setCameraPosition() {
        // get position of Camera
        tempPos = transform.position;

        // get position of Player
        playerPosition = playerObject.transform.position;        
        // get PlayerMovement variables from other script
        playerMov = playerObject.GetComponent<PlayerMovement>();

        // camera switching left or right from player according to direction player is running
        if( playerMov.runningRight == true ) {
            desiredPosition = playerPosition + cameraOffsetVectorX;
        } else if (playerMov.runningLeft == true ) {
            desiredPosition = playerPosition - cameraOffsetVectorX;
        }

        // smoothening camera
        // Lerp is a smoothening function 
        tempPos.x = Vector3.Lerp(tempPos, desiredPosition, smoothSpeed * Time.deltaTime).x;

        // follow main character on Y axis
        tempPos.y = playerPosition.y + cameraOffsetY;

        //
        // Kathrin says she want to see the ground if when jumping
        //
        //

        // apply the changes onto the camera
        transform.position = tempPos;
    }
}
