using System;
using System.Collections;
using UnityEngine;
using UObject = UnityEngine.Object;

public class PlattformDestroyer : MonoBehaviour
{
    private bool notDestroyed = true;
    private DateTime enter;
    private DateTime exit;

    private float destroyAfterSeconds;
    private float autoDestroyAfter;

    private void Start() {
        destroyAfterSeconds = 15f;
        autoDestroyAfter = 120f;

        StartCoroutine( AutoDestroy() );
    }

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

    IEnumerator AutoDestroy() {
        yield return new WaitForSeconds( autoDestroyAfter );
        DestroyPlattform();
    }

    private void DestroyPlattform() {
        UObject.Destroy( gameObject , destroyAfterSeconds );
        notDestroyed = false;
    }
}
