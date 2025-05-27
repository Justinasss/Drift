using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drive : MonoBehaviour
{
    public float speed = 1500f;
    public float turnSpeed = 50f;

    private Rigidbody rb;
    private float moveInput;
    private float turnInput;


    void Start()
    {
        GetComponent<Rigidbody>().centerOfMass = new Vector3(0, -0.5f, 0);
        rb = GetComponent<Rigidbody>();


        
    }

    void FixedUpdate()
    {
  
        moveInput = Input.GetAxis("Vertical"); 
        turnInput = Input.GetAxis("Horizontal");

        print(turnInput);


        rb.AddRelativeForce(Vector3.forward * moveInput * speed * Time.fixedDeltaTime);

        
        if (rb.velocity.magnitude > 0.1f)
        {
            float turn = turnInput * turnSpeed * Time.fixedDeltaTime;
            Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
           // rb.MoveRotation(rb.rotation * turnRotation);
        }
    }
}
