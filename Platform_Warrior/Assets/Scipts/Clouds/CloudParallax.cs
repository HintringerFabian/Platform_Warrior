using UnityEngine;
using SysRandom = System.Random;

public class CloudParallax : MonoBehaviour {

    [SerializeField]
    private GameObject background;
    [SerializeField]
    private GameObject player;

    private SpriteRenderer backgroundSR;

    private float maxEffectStrength;
    private float parallaxEffectStrength;
    private float startPos;

    void Start() {
        maxEffectStrength = 1f;
        parallaxEffectStrength = GetRandomFloatBetween(maxEffectStrength);
        startPos = transform.position.y;
        backgroundSR = background.GetComponent<SpriteRenderer>();
    }


    private void FixedUpdate() {
        Vector3 playerPos = player.transform.position;
        float dist = -( playerPos.y * parallaxEffectStrength );
        Vector3 transPos = new Vector3( transform.position.x , startPos + dist , transform.position.z );

        transform.position = transPos;

        float backPosY = background.transform.position.y;
        float backSpriteYSize = backgroundSR.bounds.size.y;

        if ( transform.position.y < backPosY - backSpriteYSize / 2) {
            transform.position = new Vector3( transPos.x , backSpriteYSize , transPos.z );
            startPos = transform.position.y;
        } else if ( transform.position.y > backPosY + backSpriteYSize / 2 ) {
            transform.position = new Vector3( transPos.x , -backSpriteYSize , transPos.z );
            startPos = transform.position.y;
        }
    }

    private float GetRandomFloatBetween(float max) {
        SysRandom random = new SysRandom();
        float number;

        do {
            number = (float) random.NextDouble();
        } while ( number < max );

        return number;
    }
}
