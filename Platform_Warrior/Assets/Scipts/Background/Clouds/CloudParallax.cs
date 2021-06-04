using UnityEngine;
using SysRandom = System.Random;

public class CloudParallax : MonoBehaviour {

    [SerializeField]
    private GameObject player, sky;

    private float startPosY, startPosX;
    private float skyLengthX, skyLengthY;
    private float skyLengthXhalv, skyLengthYhalv;
    private float parallaxStrength;

    private void Start() {

        startPosY = transform.position.y;
        startPosX = transform.position.x;

        parallaxStrength = 0.4f;

        skyLengthX = sky.GetComponent<SpriteRenderer>().bounds.size.x;
        skyLengthY = sky.GetComponent<SpriteRenderer>().bounds.size.y;
        skyLengthXhalv = skyLengthX / 2;
        skyLengthYhalv = skyLengthY / 2;
    }

    private void LateUpdate() {

        // get distance of player moved and apply a parallax effect 
        // (slower moving than the player onto it)
        Vector3 playerPos = player.transform.position;

        float distY = playerPos.y * parallaxStrength;
        float distX = playerPos.x * parallaxStrength;

        transform.position = new Vector3( startPosX + distX , startPosY + distY , playerPos.z );

        // if moving out of the sky graphics reset the position to the opposite side
        float posX = playerPos.x * ( 1 - parallaxStrength );
        float posY = playerPos.y * ( 1 - parallaxStrength );

        if ( posX > startPosX + skyLengthXhalv ) {
            startPosX += skyLengthX;

        } else if ( posX < startPosX - skyLengthXhalv ) {
            startPosX -= skyLengthX;
        }
        
        if ( posY > startPosY + skyLengthYhalv ) {
            startPosY += skyLengthY;

        } else if ( posY < startPosY - skyLengthYhalv ) {
            startPosY -= skyLengthY;
        }
    }
}
