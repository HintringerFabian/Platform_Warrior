using System;
using System.Collections;
using UnityEngine;
using UObject = UnityEngine.Object;

public class PlattformDestroyer : MonoBehaviour
{
    private bool notDestroyed;
    private DateTime enter;
    private DateTime exit;

    private float destroyAfterSeconds;
    private float autoDestroyAfter;

    private void Start() {
        notDestroyed = true;

        destroyAfterSeconds = 15f;
        autoDestroyAfter = 180f;

        // automatically destroy the plattforms after given time
        StartCoroutine( AutoDestroy() );
    }

    // automatically destroy the plattforms after given time
    IEnumerator AutoDestroy() {
        yield return new WaitForSeconds( autoDestroyAfter );
        DestroyPlattform();
    }


    // when entering and leaving the plattform a timestamp is generated
    // if you have touched the plattform by accident it will not despawn
    // (well yes but actually no) it will auto despawn after 3 mins
    private void OnCollisionEnter2D( Collision2D collision ) {
        enter = DateTime.Now;
    }

    private void OnCollisionExit2D( Collision2D collision ) {
        exit = DateTime.Now;

        TimeSpan timespan = exit - enter;

        if ( notDestroyed && (timespan.TotalSeconds > 0.5 )) {
            DestroyPlattform();
        } 
    }

    private void DestroyPlattform() {
        UObject.Destroy( gameObject , destroyAfterSeconds );
        notDestroyed = false;
    }
}
