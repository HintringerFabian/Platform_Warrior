using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyFollower : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // follow player 
        transform.position = player.transform.position;

        // need script which generates background with clouds which follows the player
        // i would like the background following WITH a parallax effect
    }
}
