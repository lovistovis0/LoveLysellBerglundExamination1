using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float accelerationSpeed;
    [SerializeField] private float maxVelocity;
    
    private Rigidbody2D playerRigidbody;
    private Vector2 playerInput;
    
    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        playerInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        playerRigidbody.AddForce(playerInput * accelerationSpeed / Mathf.Pow(Time.timeScale, 1) * Time.fixedDeltaTime / 0.02f);
        playerRigidbody.velocity = Vector2.ClampMagnitude(playerRigidbody.velocity, maxVelocity / Time.timeScale);
    }
}