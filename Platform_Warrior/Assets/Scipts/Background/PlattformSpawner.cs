using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlattformSpawner : MonoBehaviour {
    [SerializeField]
    private GameObject[] plattform;

    bool plattformSpawned;


    // Start is called before the first frame update
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
            float yOffset = System.Convert.ToSingle( Random.Range( 3 , 7 ) );
            float xOffset = (System.Convert.ToSingle( Random.Range( 5 , 11 ) )
                + ( sprite.bounds.size.x + spawnedSprite.bounds.size.x ) / 2 )
                * randomNegPos;

            spawnedPlattform.transform.position = transform.position + new Vector3( xOffset , yOffset );

            plattformSpawned = true;
        }
    }
}
