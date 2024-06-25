using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Movement : MonoBehaviour
{
    Rigidbody rocketRigidBody;

    [SerializeField] float mainThrust = 1000.0f;

    [SerializeField] float rotationRate = 1000.0f;

    void Start()
    {
        rocketRigidBody = GetComponent<Rigidbody>(); // cache ref to rigidbody component
    }

    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }

    void ProcessThrust()
    {
        if(Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            rocketRigidBody.AddRelativeForce((Vector3.up  * mainThrust) * Time.deltaTime);
        }
    }

    void ProcessRotation()
    {
        if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            ApplyRotation(rotationRate);
        }
        else if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            ApplyRotation(-rotationRate);
        }
    }

    void ApplyRotation(float rotationThisFrame)
    {
        rocketRigidBody.freezeRotation = true; // prevent contact rotation issues with physics objects
        transform.Rotate((Vector3.forward * rotationThisFrame) * Time.deltaTime);
        rocketRigidBody.freezeRotation = false; // prevent contact rotation issues with physics objects
    }
}
