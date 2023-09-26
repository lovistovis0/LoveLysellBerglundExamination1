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

        RotateTowardsMoveDirection();
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        playerRigidbody.AddForce(playerInput * accelerationSpeed / Mathf.Pow(Time.timeScale, 1) * Time.fixedDeltaTime / 0.02f);
        playerRigidbody.velocity = Vector2.ClampMagnitude(playerRigidbody.velocity, maxVelocity / Time.timeScale);
    }
    
    private void RotateTowardsMoveDirection()
    {
        // Ensure the rigidbody has a non-zero velocity before attempting to rotate.
        if (playerRigidbody.velocity != Vector2.zero)
        {
            float angle = Mathf.Atan2(playerRigidbody.velocity.y, playerRigidbody.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}
