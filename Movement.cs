using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float thrustForce = 800f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] float rotationSpeed = 100f;

    [SerializeField]public ParticleSystem mainEngineParticles;
    [SerializeField]ParticleSystem leftEngineParticles;
    [SerializeField]ParticleSystem rightEngineParticles;
    Rigidbody rb;
    AudioSource audioSource;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }
    void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            StartThrusting();

        }
        else
        {
            StopThrusting();
        }

    }

    void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.A))
        {
            RotateLeft();
        }
        else if (Input.GetKey(KeyCode.D))
        {
            RotateRight();
        }
        else
        {
            StopRotating();
        }
    }


    void StartThrusting()
    {
        // rb.AddRelativeForce((0, 1, 0) * Time.deltaTime * thrustForce);
        rb.AddRelativeForce(Vector3.up * thrustForce * Time.deltaTime);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }
        if (!mainEngineParticles.isPlaying)
        {
            mainEngineParticles.Play();
        }
    }

    void StopThrusting()
    {
        audioSource.Stop();
        mainEngineParticles.Stop();
    }

    void ApplyRotation(float rotationThisFrame)
    {
        rb.freezeRotation = true; /*Freezing the physics rotation system
        (Due to collision with other objects) so we can manually rotate*/
        transform.Rotate(Vector3.forward * Time.deltaTime * rotationThisFrame);

        rb.freezeRotation = false;
    }



    void RotateRight()
    {
        ApplyRotation(-(rotationSpeed));
        if (!leftEngineParticles.isPlaying)
        {
            leftEngineParticles.Play();
        }
    }

    void RotateLeft()
    {
        ApplyRotation((rotationSpeed));
        if (!rightEngineParticles.isPlaying)
        {
            rightEngineParticles.Play();
        }
    }

    void StopRotating()
    {
        rightEngineParticles.Stop();
        leftEngineParticles.Stop();
    }

}