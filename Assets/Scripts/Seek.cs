using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seek : MonoBehaviour
{
    [SerializeField] private float seekTValue = 0.005f;
    
    private Transform playerTransform;
    
    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        // CHALLENGE: This could be more efficient (DONE)
        transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, seekTValue);
        // put in player's position
    }
}
