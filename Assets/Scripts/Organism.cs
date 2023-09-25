using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Organism : MonoBehaviour
{
    // Options
    [Header("Movement")]
    [SerializeField] private float movementRandomRange;
    
    [Header("StartScale")]
    [SerializeField] private float minStartScale = 1;
    [SerializeField] private float maxStartScale = 1;

    [Header("ScaleChanger")]
    [SerializeField] private bool enableScaleChanger;
    [SerializeField] private string tagToScale;
    [SerializeField] private float scaleChangeMin;
    [SerializeField] private float scaleChangeMax;
    
    [Header("Seek")]
    [SerializeField] private bool enableSeek;
    [SerializeField] private float seekTValue = 0.005f;

    [Header("Wobble")]
    [SerializeField] private bool enableWobble;
    [SerializeField] private float wobbleSpeed = 1f;
    [SerializeField] private float maximumWobbleSize = 10f;
    
    // Static References
    private Rigidbody2D organismRigidbody;
    private Transform playerTransform;
    
    // Start is called before the first frame update
    private void Start()
    {
        organismRigidbody = GetComponent<Rigidbody2D>();
        playerTransform = GameObject.FindWithTag("Player").transform;

        SetStartingScale();
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (enableScaleChanger && other.gameObject.CompareTag(tagToScale))
        {
            other.transform.localScale *= Random.Range(scaleChangeMin, scaleChangeMax);
            other.gameObject.tag = tag;
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        Movement();
        if (enableWobble) Wobble();
        if (enableSeek) Seek();
    }

    private void SetStartingScale()
    {
        float scale = Random.Range(minStartScale, maxStartScale);
        transform.localScale = new Vector3(scale, scale, 1);
    }

    private void Seek()
    {
        // CHALLENGE: This could be more efficient (DONE)
        organismRigidbody.MovePosition(Vector3.MoveTowards(organismRigidbody.position, playerTransform.position, seekTValue));
    }

    private void Movement()
    {
        organismRigidbody.MovePosition(organismRigidbody.position + new Vector2(Random.Range(-movementRandomRange, movementRandomRange), Random.Range(-movementRandomRange,movementRandomRange))); 
    }

    private void Wobble()
    {
        transform.localScale = transform.localScale + new Vector3(Random.Range(-.002f,.002f), Random.Range(-.002f,.002f), 0) * wobbleSpeed;
        transform.localScale = Vector3.ClampMagnitude(transform.localScale, maximumWobbleSize);
    }
}