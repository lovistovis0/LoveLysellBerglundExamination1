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
    [SerializeField] private float maximumRotationSpeed;
    
    [Header("StartScale")]
    [SerializeField] private float minStartScale = 1;
    [SerializeField] private float maxStartScale = 1;

    [Header("ScaleChanger")]
    [SerializeField] private bool enableScaleChanger;
    [SerializeField] private string tagToScale;
    [SerializeField] private float scaleToEffectMin;
    [SerializeField] private float scaleToEffectMax;
    [SerializeField] private float scaleChangeMin;
    [SerializeField] private float scaleChangeMax;
    
    [Header("Seek")]
    [SerializeField] private bool enableSeek;
    [SerializeField] private float seekTValue = 0.005f;

    [Header("Wobble")]
    [SerializeField] private bool enableWobble;
    [SerializeField] private float wobbleSpeed = 1f;

    // Static References
    private Rigidbody2D organismRigidbody;
    private Transform playerTransform;
    
    // Start is called before the first frame update
    private void Start()
    {
        organismRigidbody = GetComponent<Rigidbody2D>();
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null) playerTransform = player.transform;

        SetStartingScale();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (enableScaleChanger &&
            other.gameObject.CompareTag(tagToScale) &&
            other.transform.localScale.magnitude > scaleToEffectMin &&
            other.transform.localScale.magnitude < scaleToEffectMax
            )
        {
            other.transform.localScale *= Random.Range(scaleChangeMin, scaleChangeMax);
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        Movement();
        LimitRotationSpeed();
        if (enableWobble) Wobble();
        if (enableSeek && playerTransform != null) Seek();
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

    private void LimitRotationSpeed()
    {
        organismRigidbody.angularVelocity = Mathf.Clamp(organismRigidbody.angularVelocity, -maximumRotationSpeed, maximumRotationSpeed);
    }

    private void Wobble()
    {
        transform.localScale = transform.localScale + new Vector3(Random.Range(-.002f,.002f), Random.Range(-.002f,.002f), 0) * wobbleSpeed;
        
        float averageScale = (transform.localScale.x + transform.localScale.y) * 0.5f;
        transform.localScale = new Vector3(
            Mathf.Clamp(transform.localScale.x, averageScale * 0.5f, averageScale * 1.5f),
            Mathf.Clamp(transform.localScale.y, averageScale * 0.5f, averageScale * 1.5f),
            transform.localScale.z
        );
    }
}