using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class Movement : MonoBehaviour
{
    Rigidbody rocketRigidBody;
    AudioSource rocketAudio;
    [SerializeField] ParticleSystem mainThrusterParticles;
    [SerializeField] ParticleSystem leftThrusterParticles;
    [SerializeField] ParticleSystem rightThrusterParticles;
    [SerializeField] float mainThrust = 1000.0f;
    [SerializeField] float rotationRate = 1000.0f;
    [SerializeField] AudioClip thrusterAudio;

    void Start()
    {
        rocketRigidBody = GetComponent<Rigidbody>(); // cache ref to rigidbody component
        rocketAudio = GetComponent<AudioSource>();
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
            StartThrust();
        }
        else
        {
            rocketAudio.Stop();
            mainThrusterParticles.Stop();
        }
    }

    void StartThrust()
    {
        rocketRigidBody.AddRelativeForce((Vector3.up * mainThrust) * Time.deltaTime);
        if (!rocketAudio.isPlaying)
        {
            rocketAudio.PlayOneShot(thrusterAudio);
        }

        if (mainThrusterParticles.isStopped)
        {
            mainThrusterParticles.Play();
        }
    }

    void ProcessRotation()
    {
        RotateLeft();
        RotateRight();
    }

    void RotateRight()
    {
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            ApplyRotation(-rotationRate);
            if (rightThrusterParticles.isStopped)
            {
                rightThrusterParticles.Play();
            }
        }
        else
        {
            rightThrusterParticles.Stop();
        }
    }

    void RotateLeft()
    {
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            ApplyRotation(rotationRate);
            if (leftThrusterParticles.isStopped)
            {
                leftThrusterParticles.Play();
            }
        }
        else
        {
            leftThrusterParticles.Stop();
        }
    }

    void ApplyRotation(float rotationThisFrame)
    {
        rocketRigidBody.freezeRotation = true; // prevent contact rotation issues with physics objects
        transform.Rotate((Vector3.forward * rotationThisFrame) * Time.deltaTime);
        rocketRigidBody.freezeRotation = false; // prevent contact rotation issues with physics objects
    }
}
