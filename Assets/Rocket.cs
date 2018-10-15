using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

    Rigidbody rigidBody;
    AudioSource audioSource;
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 25f;

    // Use this for initialization
    void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
        ProcessInput();
	}

    void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                print("Friendly");
                break;
            case "Fuel":
                print("Fuel");
                break;
            case "Respawn":
                print("Respawn");
                break;
            case "Finish":
                print("Finish");
                break;
            default:
                KillYou();
                break;
        }
    }

    private void KillYou()
    {
        throw new NotImplementedException();
    }

    private void ProcessInput()
    {
        bool isWPressed = Input.GetKey(KeyCode.W);

        if (isWPressed)
        {
            Accelerate();
        }
        else
        {
            Deaccelerate();
        }
        Rotate();
    }

    private void Deaccelerate()
    {
        audioSource.Stop();
    }

    private void Accelerate()
    {
        rigidBody.AddRelativeForce(Vector3.up * mainThrust);
        if (!audioSource.isPlaying)
            audioSource.Play();
    }

    private void Rotate()
    {
        bool isAPressed = Input.GetKey(KeyCode.A);
        bool isDPressed = Input.GetKey(KeyCode.D);

        if (isAPressed && isDPressed)
            return;

        float rotationSpeed = rcsThrust * Time.deltaTime;

        rigidBody.freezeRotation = true;

        if (isAPressed)
            transform.Rotate(Vector3.forward * rotationSpeed);

        if (isDPressed)
            transform.Rotate(Vector3.back * rotationSpeed);

        rigidBody.freezeRotation = false;
    }
}
