using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlattformSpawner : MonoBehaviour {
    [SerializeField]
    private GameObject[] plattform;

    bool plattformSpawned;

    private const int minYDist = 3;
    private const int maxYDist = 7;
    private const int minXDist = 5;
    private const int maxXDist = 11;

    void Start() {

        plattformSpawned = false;
    }

    private void OnCollisionEnter2D( Collision2D collision ) {

        if ( !plattformSpawned ) {
            int plattformNr = Random.Range( 0 , plattform.Length );

            SpriteRenderer sprite = GetComponent<SpriteRenderer>();
            GameObject spawnedPlattform = Instantiate( plattform[plattformNr] );
            SpriteRenderer spawnedSprite = spawnedPlattform.GetComponent<SpriteRenderer>();

            int randomNegPos = ( Random.Range( 0 , 2 ) * 2 ) - 1;
            float yOffset = System.Convert.ToSingle( Random.Range( minYDist , maxYDist ) );
            float xOffset = (System.Convert.ToSingle( Random.Range( minXDist , maxXDist ) )
                + ( sprite.bounds.size.x + spawnedSprite.bounds.size.x ) / 2 )
                * randomNegPos;

            spawnedPlattform.transform.position = transform.position + new Vector3( xOffset , yOffset );

            plattformSpawned = true;

            // todo 
            // implement plattform despawning after a few seconds 
            // despawn 15 seconds after enemy has been slain, ... think of something
        }
    }
}
