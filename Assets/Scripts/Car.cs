using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public WheelCollider wheelFL, wheelFR, wheelRL, wheelRR;
    public Transform meshFL, meshFR, meshRL, meshRR;

    public float motorPower = 1500f;
    public float maxSteerAngle = 25f;

    public float normalStiffness = 1.5f;
    public float driftStiffness = 0.3f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float steer = Input.GetAxis("Horizontal") * maxSteerAngle;
        float accel = Input.GetAxis("Vertical") * motorPower;

        wheelFL.steerAngle = steer;
        wheelFR.steerAngle = steer;

        wheelRL.motorTorque = accel;
        wheelRR.motorTorque = accel;

        
        bool drifting = Input.GetKey(KeyCode.Space);
        float targetStiffness = drifting ? driftStiffness : normalStiffness;

        ApplyDrift(wheelFL, targetStiffness);
        ApplyDrift(wheelFR, targetStiffness);
        ApplyDrift(wheelRL, targetStiffness);
        ApplyDrift(wheelRR, targetStiffness);

        //UpdateWheel(wheelFL, meshFL);
        //UpdateWheel(wheelFR, meshFR);
        //UpdateWheel(wheelRL, meshRL);
        //UpdateWheel(wheelRR, meshRR);
    }

    void ApplyDrift(WheelCollider col, float stiffness)
    {
        WheelFrictionCurve sidewaysFriction = col.sidewaysFriction;
        sidewaysFriction.stiffness = stiffness;
        col.sidewaysFriction = sidewaysFriction;
    }

    void UpdateWheel(WheelCollider col, Transform mesh)
    {
        Vector3 pos;
        Quaternion rot;
        col.GetWorldPose(out pos, out rot);

        pos.y += 0.05f;  

        mesh.position = pos;
        mesh.rotation = rot;
    }
}
