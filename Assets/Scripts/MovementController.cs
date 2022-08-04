using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(Rigidbody))]
public class MovementController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10;
    [SerializeField] private float maxVelocity = 3;
    [SerializeField] private float turnSpeed = 1;
    [SerializeField] private float maxTrackdOffset = 3;
    private Rigidbody rb;
    private Joystick joystick;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        joystick = FindObjectOfType <Joystick>();
    }

    void FixedUpdate()
    {
        if (joystick == null || rb == null)
            return;

        if (joystick.Horizontal == 0 && joystick.Vertical == 0)
        {
            if (Vector3.Dot(rb.velocity.normalized, transform.forward) > 0.35f)
                rb.velocity = transform.forward * rb.velocity.magnitude;
            rb.velocity *= (1 - Time.fixedDeltaTime * 5);
        }
        else
        { 
            var inputDir = new Vector3(joystick.Horizontal, 0, joystick.Vertical);
         //   rb.rotation = Quaternion.Slerp(rb.rotation, Quaternion.LookRotation(inputDir), turnSpeed * Time.deltaTime);
            rb.rotation = Quaternion.Lerp(rb.rotation, Quaternion.LookRotation(inputDir), turnSpeed * Time.fixedDeltaTime);

            rb.AddForce(transform.forward * inputDir.magnitude * moveSpeed * Time.fixedDeltaTime, ForceMode.VelocityChange);
            if (Vector3.Dot(rb.velocity.normalized, transform.forward) > 0.35f)
                rb.velocity = transform.forward * rb.velocity.magnitude;
            if (rb.velocity.magnitude > maxVelocity)
                rb.velocity = rb.velocity.normalized * maxVelocity;
        }

    }
}
