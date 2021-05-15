using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float horsepower = 20000.0f;
    [SerializeField] GameObject centerOfMass;
    [SerializeField] private float turnspeed = 45.0f;
    [SerializeField] private TextMeshProUGUI speedometerText;
    private float speed;
    [SerializeField] private TextMeshProUGUI rpmText;
    private float rpm;
    private WheelCollider[] wheelColliders;
    private float wheelCircumference;
    private int wheelsOnGround = 0;

    private float verticalInput;
    private float horizontalInput;

    private Rigidbody playerRb;

    private void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerRb.centerOfMass = centerOfMass.transform.localPosition;

        wheelColliders = GetComponentsInChildren<WheelCollider>();

        //circumference of the wheel = 2𝛑r where 𝛑 = 3.141593 and r = radius of the wheel
        wheelCircumference = 2.0f * Mathf.PI * wheelColliders[0].radius;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        wheelsOnGround = wheelColliders.Aggregate(0, (count, collider) => collider.isGrounded ? count + 1 : count);
        
        if (IsOnGround())
        {
            // Move the vehicle forward
            verticalInput = Input.GetAxis("Vertical");
            float horsepowerScale = horsepower * verticalInput;
            playerRb.AddRelativeForce(Vector3.forward * horsepowerScale);
            
            // Turn the vehicle
            horizontalInput = Input.GetAxis("Horizontal");
            float turnspeedscale = Time.deltaTime * turnspeed * horizontalInput;
            transform.Rotate(Vector3.up * turnspeedscale);
        }
    }

    void Update()
    {
        if (IsOnGround())
        {
            speed = Mathf.Round(playerRb.velocity.magnitude * 3.6f);
            speedometerText.text = $"Speed: {speed} km/h";

            // rpm = Mathf.Round((playerRb.velocity.magnitude * 60) / wheelCircumference);
            // rpmText.text = $"RPM: {rpm}";

            // https://docs.unity3d.com/ScriptReference/WheelCollider-rpm.html
            rpmText.text = $"RPM: {Mathf.Round(wheelColliders[0].rpm)}";
        }
    }

    bool IsOnGround()
    {
        return wheelsOnGround == wheelColliders.Length;
    }
}