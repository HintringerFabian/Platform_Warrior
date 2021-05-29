using UnityEngine;
using SysRandom = System.Random;

public class CloudParallax : MonoBehaviour {

    [SerializeField]
    private GameObject player;

    private float xPlayerDistance;
    private float startPosY;
    private float startPosX;

    private float parallaxStrength;

    private void Start() {
        xPlayerDistance = transform.position.x - player.transform.position.x;
        startPosY = transform.position.y;
        startPosX = transform.position.x;
        parallaxStrength = 0.4f;
    }

    private void LateUpdate() {
        Vector3 playerPos = player.transform.position;

        float distY = playerPos.y * parallaxStrength;
        float distX = playerPos.x * parallaxStrength;

        float cloudX = startPosX + distX;
        float cloudY = startPosY + distY;

        transform.position = new Vector3( cloudX , cloudY , playerPos.z );


        // if cloud is too low, too high, too much right or left
        // change position of clouds
    }
}
